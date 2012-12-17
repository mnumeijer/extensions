﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Utilities.Reflection;
using System.Reflection;
using Signum.Entities.SMS;
using Signum.Windows.Operations;
using Signum.Entities;

namespace Signum.Windows.SMS
{
    public static class SMSClient
    {
        public static void AsserIsStarted()
        {
            Navigator.Manager.AssertDefined(ReflectionTools.GetMethodInfo(() => SMSClient.Start()));
        }


        public static void Start()
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                Navigator.AddSettings(new List<EntitySettings>
                {
                    new EntitySettings<SMSMessageDN> { View = e => new SMSMessage() },
                    new EntitySettings<SMSTemplateDN> { View = e => new SMSTemplate() },
                    new EntitySettings<SMSSendPackageDN> { View = e => new SMSSendPackage()},
                    new EntitySettings<SMSUpdatePackageDN> { View = e => new SMSUpdatePackage()},
                });

                OperationClient.AddSetting(new EntityOperationSettings(SMSMessageOperation.CreateSMSMessageFromTemplate)
                {
                    Click = FindAssociatedTemplates
                });
            }
        }

        public static IdentifiableEntity FindAssociatedTemplates(EntityOperationContext e)
        {
            var template = Navigator.Find(new FindOptions(typeof(SMSTemplateDN))
            {
                FilterOptions = new List<FilterOption>
                {
                    { new FilterOption("IsActive", true) { Frozen = true } },
                    { new FilterOption("AssociatedType", Server.ServerTypes[e.Entity.GetType()]) }
                },
                SearchOnLoad = true,
            });

            if (template != null)
                Navigator.Navigate(e.Entity.ToLite().ConstructFromLite<SMSMessageDN>(SMSMessageOperation.CreateSMSMessageFromTemplate,
                    ((Lite<SMSTemplateDN>)template).Retrieve()));

            return null;
        }
    }
}
