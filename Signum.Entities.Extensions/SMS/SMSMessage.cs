﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signum.Entities.SMS
{
    public enum SMSMessageState
    {
        Created,
        Sent
    }

    public enum SendState
    { 
        None,
        Delivered,
        Failed,
        Queued,
        Sent    
    }

    public enum SMSMessageOperations
    { 
        Create,
        Send,
        UpdateStatus
    }

    [Serializable]
    public class SMSMessageDN : Entity
    {
        Lite<SMSTemplateDN> template;
        public Lite<SMSTemplateDN> Template
        {
            get { return template; }
            set { Set(ref template, value, () => Template); }
        }

        string message;
        [StringLengthValidator(AllowNulls = false, Max = SMSCharacters.SMSMaxTextLength)]
        public string Message
        {
            get { return message; }
            set { Set(ref message, value, () => Message); }
        }

        string from;
        [StringLengthValidator(AllowNulls = false)]
        public string From
        {
            get { return from; }
            set { Set(ref from, value, () => From); }
        }

        DateTime? sendDate;
        [SecondsPrecissionValidator]
        public DateTime? SendDate
        {
            get { return sendDate; }
            set { Set(ref sendDate, value, () => SendDate); }
        }

        SMSMessageState state = SMSMessageState.Created;
        public SMSMessageState State
        {
            get { return state; }
            set { Set(ref state, value, () => State); }
        }

        SendState sendState;
        public SendState SendState
        {
            get { return sendState; }
            set { Set(ref sendState, value, () => SendState); }
        }

        string sourceNumber;
        public string SourceNumber
        {
            get { return sourceNumber; }
            set { Set(ref sourceNumber, value, () => SourceNumber); }
        }

        string destinationNumber;
        [StringLengthValidator(AllowNulls=false)]
        public string DestinationNumber
        {
            get { return destinationNumber; }
            set { Set(ref destinationNumber, value, () => DestinationNumber); }
        }
    }
}