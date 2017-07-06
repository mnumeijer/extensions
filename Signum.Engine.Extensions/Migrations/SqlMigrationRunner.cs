﻿using Signum.Engine.Cache;
using Signum.Engine.Maps;
using Signum.Entities;
using Signum.Entities.Migrations;
using Signum.Utilities;
using Signum.Utilities.DataStructures;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Signum.Engine.Migrations
{
    public class SqlMigrationRunner
    {
        public static string MigrationsDirectory = @"..\..\Migrations";
        public static string MigrationsDirectoryName = "Migrations";

        public static void SqlMigrations()
        {
            SqlMigrations(autoRun: false);
        }

        public static void SqlMigrations(bool autoRun)
        {
            while (true)
            {
                List<MigrationInfo> list = ReadMigrationsDirectory();

                SetExecuted(list);

                if (!Prompt(list, autoRun) || autoRun)
                    return;
            }
        }

        private static void SetExecuted(List<MigrationInfo> migrations)
        {
            MigrationLogic.EnsureMigrationTable<SqlMigrationEntity>();

            var first = migrations.FirstOrDefault();

            var executedMigrations = Database.Query<SqlMigrationEntity>().Select(m => m.VersionNumber).OrderBy().ToList().Where(d => first == null || first.Version.CompareTo(d) <= 0).ToList();

            var dic = migrations.ToDictionary(a => a.Version, "Migrations in folder");

            foreach (var ver in executedMigrations)
            {
                var m = dic.TryGetC(ver);
                if (m != null)
                    m.IsExecuted = true;
                else
                    migrations.Add(new MigrationInfo
                    {
                        FileName = null,
                        Comment = ">> In Database Only <<",
                        IsExecuted = true,
                        Version = ver
                    });

            }

            migrations.Sort(a => a.Version);
        }

        private static List<MigrationInfo> ReadMigrationsDirectory()
        {
            Console.WriteLine();
            SafeConsole.WriteLineColor(ConsoleColor.DarkGray, "Reading migrations from: " + MigrationsDirectory);

            if (!Directory.Exists(MigrationsDirectory))
            {
                Directory.CreateDirectory(MigrationsDirectory);
                SafeConsole.WriteLineColor(ConsoleColor.White, "Directory " + MigrationsDirectory + " auto-generated...");
            }

            Regex regex = new Regex(@"(?<version>\d{4}\.\d{2}\.\d{2}\-\d{2}\.\d{2}\.\d{2})(_(?<comment>.+))?\.sql");

            var matches =  Directory.EnumerateFiles(MigrationsDirectory, "*.sql")
                .Select(fileName => new { fileName, match = regex.Match(Path.GetFileName(fileName)) }).ToList();

            var errors = matches.Where(a => !a.match.Success);

            if (errors.Any())
                throw new InvalidOperationException("Some scripts in the migrations directory have an invalid format (yyyy.MM.dd-HH.mm.ss_OptionalComment.sql) " +
                    errors.ToString(a => Path.GetFileName(a.fileName), "\r\n"));

            var list = matches.Select(a => new MigrationInfo
            {
                FileName = a.fileName,
                Version = a.match.Groups["version"].Value,
                Comment = a.match.Groups["comment"].Value,
            }).OrderBy(a => a.Version).ToList();
            
            return list;
        }

        public const string DatabaseNameReplacement = "$DatabaseName$";

        private static bool Prompt(List<MigrationInfo> migrations, bool autoRun)
        {
            Draw(migrations, null);

            if (migrations.Any(a => a.IsExecuted && a.FileName == null))
            {
                var str = "There are fresh executed migrations that are not in the folder. Get latest version?";
                if (autoRun)
                    throw new InvalidOperationException(str);

                SafeConsole.WriteLineColor(ConsoleColor.Red, str);
                return false;
            }


            if (migrations.SkipWhile(a => a.IsExecuted).Any(a => a.IsExecuted))
            {
                var str = "Possible merge conflict. There are old migrations in the folder that have not been executed!. You need to manually discard one migration branch.";
                if (autoRun)
                    throw new InvalidOperationException(str);

                SafeConsole.WriteLineColor(ConsoleColor.Red, str);
                Console.WriteLine();
                Console.Write("Write '");
                SafeConsole.WriteColor(ConsoleColor.White, "force");
                Console.WriteLine("' to execute them anyway");

                if (Console.ReadLine() != "force")
                    return false;
            }

            var last = migrations.LastOrDefault() ?? null;
            if (migrations.All(a => a.IsExecuted))
            {
                if (autoRun || !SafeConsole.Ask("Create new migration?"))
                    return false;

                var script = Schema.Current.SynchronizationScript(interactive: true, replaceDatabaseName: DatabaseNameReplacement);

                if (script == null)
                {
                    SafeConsole.WriteLineColor(ConsoleColor.Green, "No changes found!");
                    return false;
                }
                else
                {
                    string version = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss");

                    string comment = SafeConsole.AskString("Comment for the new Migration? ", stringValidator: s => null);

                    string fileName = version + (comment.HasText() ? "_" + FileNameValidatorAttribute.RemoveInvalidCharts(comment) : null) + ".sql";

                    File.WriteAllText(Path.Combine(MigrationsDirectory, fileName), script.ToString());

                    AddCsprojReference(fileName);
                }

                return true;
            }
            else
            {
                if (!autoRun && !SafeConsole.Ask("Run {0} migration(s)?".FormatWith(migrations.Count(a => !a.IsExecuted))))
                    return false;

                try
                {
                    foreach (var item in migrations.AsEnumerable().Where(a => !a.IsExecuted))
                    {
                        Draw(migrations, item);

                        Execute(item, autoRun);
                    }

                    return true;
                }
                catch (MigrationException)
                {
                    if (autoRun)
                        throw;

                    return true;
                }
            }
        }

        public static int Timeout = 5 * 60; 

        private static void Execute(MigrationInfo mi, bool autoRun)
        {
            string title = mi.Version + (mi.Comment.HasText() ? " ({0})".FormatWith(mi.Comment) : null);

            string databaseName = Connector.Current.DatabaseName();

            using (Connector.CommandTimeoutScope(Timeout))
            using (Transaction tr = new Transaction())
            {
                string text = File.ReadAllText(mi.FileName);

                text = text.Replace(DatabaseNameReplacement, databaseName);

                var parts = Regex.Split(text, " *GO *\r?\n", RegexOptions.IgnoreCase).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();

                int pos = 0;

                try
                {   
                    for (pos = 0; pos < parts.Length; pos++)
			        {
                        if (autoRun)
                            Executor.ExecuteNonQuery(parts[pos]);
                        else
                            SafeConsole.WaitExecute("Executing {0} [{1}/{2}]".FormatWith(title, pos + 1, parts.Length), () => Executor.ExecuteNonQuery(parts[pos]));
			        }
                }
                catch (SqlException e)
                {
                    Console.WriteLine();
                    Console.WriteLine();

                    var list = text.Lines();

                    var lineNumer = (e.LineNumber - 1) + pos + parts.Take(pos).Sum(a => a.Lines().Length);

                    SafeConsole.WriteLineColor(ConsoleColor.Red, "ERROR:");

                    var min = Math.Max(0, lineNumer - 20);
                    var max = Math.Min(list.Length - 1, lineNumer + 20);

                    if (min > 0)
                        Console.WriteLine("...");

                    for (int i = min; i <= max; i++)
                    {
                        Console.Write(i + ": ");
                        SafeConsole.WriteLineColor(i == lineNumer ? ConsoleColor.Red : ConsoleColor.DarkRed, list[i]);
                    }

                    if (max < list.Length - 1)
                        Console.WriteLine("...");

                    Console.WriteLine();
                    SafeConsole.WriteLineColor(ConsoleColor.DarkRed, e.GetType().Name + (e is SqlException ? " (Number {0}): ".FormatWith(((SqlException)e).Number) : ": "));
                    SafeConsole.WriteLineColor(ConsoleColor.Red, e.Message);

                    Console.WriteLine();

                    throw new MigrationException();
                }

                SqlMigrationEntity m = new SqlMigrationEntity
                {
                    VersionNumber = mi.Version,
                }.Save();

                mi.IsExecuted = true;

                tr.Commit();
            }
        }

        private static void Draw(List<MigrationInfo> migrationsInOrder, MigrationInfo current)
        {
            Console.WriteLine();

            foreach (var mi in migrationsInOrder)
            {
                ConsoleColor color = current == mi ? ConsoleColor.Green :
                    mi.FileName != null && mi.IsExecuted ? ConsoleColor.DarkGreen :
                    mi.FileName == null && mi.IsExecuted ? ConsoleColor.Red :
                    mi.FileName != null && !mi.IsExecuted ? ConsoleColor.White :
                    new InvalidOperationException().Throw<ConsoleColor>();


                SafeConsole.WriteColor(color,  
                    mi.IsExecuted?  "- " : 
                    current == mi ? "->" : 
                                    "  ");
                
                SafeConsole.WriteColor(color, mi.Version);
                SafeConsole.WriteLineColor(mi.FileName == null ? ConsoleColor.Red: ConsoleColor.Gray, " " + mi.Comment);
            }

            Console.WriteLine();
        }

        private static void AddCsprojReference(string fileName)
        {
            string csproj = Directory.EnumerateFiles(Path.Combine(MigrationsDirectory, ".."), "*Load.csproj").SingleEx(() => "Load.csproj");

            var doc = XDocument.Load(csproj);
            var xmlns = (XNamespace)doc.Document.Root.Attribute("xmlns").Value;

            var element = new XElement(xmlns + "Content",
               new XAttribute("Include", @"Migrations\" + fileName));

            var itemGroups = doc.Document.Root.Elements(xmlns + "ItemGroup");

            var lastContent = itemGroups
                .SelectMany(e => e.Elements(xmlns + "Content").Where(a => a.Attribute("Include").Value.StartsWith(@"Migrations\")))
                .LastOrDefault();

            if (lastContent != null)
                lastContent.AddAfterSelf(element);
            else
                itemGroups.Last().Add(element);

            doc.Save(csproj);
        }

        public class MigrationInfo
        {
            public string FileName;
            public string Version;
            public string Comment;
            public bool IsExecuted;

            public override string ToString()
            {
                return Version;
            }

        }
    }
}
