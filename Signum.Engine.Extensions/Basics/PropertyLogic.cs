﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities.Basics;
using Signum.Entities;
using Signum.Entities.Reflection;
using System.Reflection;
using Signum.Utilities;
using Signum.Engine.Maps;
using Signum.Utilities.Reflection;

namespace Signum.Engine.Basics
{
    public static class PropertyLogic
    {
        public static void Start(SchemaBuilder sb)
        {
            if (sb.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                TypeLogic.Start(sb, true);
                sb.Include<PropertyDN>();

                sb.Schema.Synchronizing += SyncronizeProperties;
            }
        }

        const string FieldsForKey = "Properties For:{0}";
        static SqlPreCommand SyncronizeProperties(Replacements replacements)
        {
            var current = Administrator.TryRetrieveAll<PropertyDN>(replacements).AgGroupToDictionary(a => a.Type.ClassName, g => g.ToDictionary(f => f.Name));

            var should = TypeLogic.TryDNToType(replacements).SelectDictionary(dn => dn.ClassName, (dn, t) => GenerateProperties(dn, t).ToDictionary(f => f.Name));

            Table table = Schema.Current.Table<PropertyDN>();

            return Synchronizer.SyncronizeCommands(
                current, should,
                null, null,
                (tn, dicCurr, dicShould) =>
                    Synchronizer.SyncronizeReplacing(replacements, FieldsForKey.Formato(tn),
                    dicCurr,
                    dicShould,
                    (fn, c) => table.DeleteSqlSync(c),
                    null,
                    (fn, c, s) =>
                    {
                        c.Name = s.Name;
                        return table.UpdateSqlSync(c);
                    },
                    Spacing.Simple), Spacing.Double);
        }

        public static List<PropertyDN> RetrieveOrGenerateProperty(TypeDN typeDN)
        {
            var current = Database.Query<PropertyDN>().Where(f => f.Type == typeDN).ToDictionary(a => a.Name);
            var total = GenerateProperties(typeDN, TypeLogic.DnToType[typeDN]).ToDictionary(a => a.Name);

            total.SetRange(current);
            return total.Values.ToList();
        }

        private static List<PropertyDN> GenerateProperties(TypeDN typeDN, Type type)
        {
            return Reflector.InstancePropertiesInOrder(type)
                .GroupBy(a => a.Name).Select(g => g.First()) //Overriden properties
                .Where(p => !Attribute.IsDefined(p, typeof(HiddenPropertyAttribute)))
                .SelectMany(pi =>
                {
                    PropertyDN property = new PropertyDN { Type = typeDN, Name = pi.Name };

                    if (Reflector.IsEmbeddedEntity(pi.PropertyType))
                    {
                        var stack = GenerateAllEmbeddedFields(typeDN, pi.PropertyType, pi.Name + ".");
                        return stack.PreAnd(property);
                    }

                    if (Reflector.IsMList(pi.PropertyType))
                    {
                        Type colType = ReflectionTools.CollectionType(pi.PropertyType);
                        if (Reflector.IsEmbeddedEntity(colType))
                        {
                            var stack = GenerateAllEmbeddedFields(typeDN, colType, pi.Name + "/");
                            return stack.PreAnd(property);
                        }
                    }

                    return new[] { property };
                }).ToList();
        }

        static List<PropertyDN> GenerateAllEmbeddedFields(TypeDN typeDN, Type type, string prefix)
        {
            return Reflector.InstancePropertiesInOrder(type)
                .GroupBy(a => a.Name).Select(g => g.First()) //Overriden properties
                .Where(p => !Attribute.IsDefined(p, typeof(HiddenPropertyAttribute)))
                .SelectMany(pi =>
                {
                    PropertyDN field = new PropertyDN { Type = typeDN, Name = prefix + pi.Name };

                    if (Reflector.IsEmbeddedEntity(pi.PropertyType))
                    {
                        var list = GenerateAllEmbeddedFields(typeDN, pi.PropertyType, prefix + pi.Name + ".");
                        return list.PreAnd(field);
                    }

                    return new[] { field };
                }).ToList();
        }
    }
}
