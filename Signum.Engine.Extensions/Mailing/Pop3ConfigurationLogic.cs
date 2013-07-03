﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using Signum.Engine.Basics;
using Signum.Engine.DynamicQuery;
using Signum.Engine.Mailing.Pop3;
using Signum.Engine.Maps;
using Signum.Engine.Operations;
using Signum.Engine.Scheduler;
using Signum.Entities;
using Signum.Entities.Mailing;
using Signum.Utilities;

namespace Signum.Engine.Mailing
{
    public static class Pop3ConfigurationLogic
    {
        public static WarningHandler Warning;
        public static TraceHandler Trace;

        public static void Start(SchemaBuilder sb, DynamicQueryManager dqm)
        {
            if (sb.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                sb.Include<Pop3ConfigurationDN>();

                dqm.RegisterQuery(typeof(Pop3ConfigurationDN), () =>
                    from s in Database.Query<Pop3ConfigurationDN>()
                    select new
                    {
                        Entity = s,
                        s.Id,
                        s.Name,
                        s.Host,
                        s.Port,
                        s.Username,
                        s.Password,
                        s.EnableSSL
                    });

                new Graph<Pop3ConfigurationDN>.Execute(Pop3ConfigurationOperation.Save)
                {
                    AllowsNew = true,
                    Lite = false,
                    Execute = (e, _) => { }
                }.Register();

                new Graph<Pop3ConfigurationDN>.Execute(Pop3ConfigurationOperation.ReceiveEmails)
                {
                    AllowsNew = true,
                    Lite = false,
                    Execute = (e, _) => e.ReceiveEmails()
                }.Register();

                SchedulerLogic.ExecuteTask.Register((Pop3ConfigurationDN smtp) =>smtp.ReceiveEmails());

                ActionTaskLogic.Register(Pop3ConfigurationAction.ReceiveAllActivePop3Configurations, () =>
                {
                    foreach (var item in Database.Query<Pop3ConfigurationDN>().Where(a=>a.Active).ToList())
                    {
                        item.ReceiveEmails();
                    }
                }); 
            }
        }

        public static void ReceiveEmails(this Pop3ConfigurationDN config)
        {
            Pop3ReceptionDN reception = new Pop3ReceptionDN { StartDate = TimeZoneManager.Now };

            try
            {
                Pop3MimeClient client = new Pop3MimeClient(config.Host, config.Port, config.EnableSSL, config.Username, config.Password) { ReadTimeout = config.ReadTimeout };

                if (Warning != null)
                    client.Warning += Warning;

                if (Trace != null)
                    client.Trace += Trace;

                int numberOfMails;
                int mailboxSize;
                if (!client.GetMailboxStats(out numberOfMails, out mailboxSize))
                    throw new Pop3Exception("Error retrieving mailbox stats");

                reception.NumberOfMails = numberOfMails;
                reception.MailboxSize = mailboxSize; 

                int maxids = Math.Min(numberOfMails, config.MaxDownloadEmails);

                for (int i = 1; i <= maxids; i++)
                {
                    RxMailMessage mm;
                    if (client.GetEmail(i, out mm))
                    {
                        var email = ToEmailMessage(mm);

                        if (AssociateWithEntities != null)
                            AssociateWithEntities(email);

                        email.Save();
                    }
                }

                reception.EndDate = TimeZoneManager.Now;
                reception.Save(); 
            }
            catch (Exception e)
            {
                var ex = e.LogException();

                try
                {
                    reception.EndDate = TimeZoneManager.Now;
                    reception.Exception = ex.ToLite();
                    reception.Save();
                }
                catch { }
            }
        }

        public static Action<EmailMessageDN> AssociateWithEntities;

        private static EmailMessageDN ToEmailMessage(RxMailMessage mm)
        {
            return new EmailMessageDN
            {
                EditableMessage = false,
                From = new EmailAddressDN(mm.From),
                Recipients =
                   mm.To.Select(ma => new EmailRecipientDN(ma, EmailRecipientKind.To)).Concat(
                   mm.CC.Select(ma => new EmailRecipientDN(ma, EmailRecipientKind.CC))).Concat(
                   mm.Bcc.Select(ma => new EmailRecipientDN(ma, EmailRecipientKind.Bcc))).ToMList(),
                State = EmailMessageState.Received,
                IsBodyHtml = mm.IsBodyHtml,
                Subject = mm.Subject,
                Body = mm.Body,
                Received = TimeZoneManager.Now,
            }; 
        }
    }
}