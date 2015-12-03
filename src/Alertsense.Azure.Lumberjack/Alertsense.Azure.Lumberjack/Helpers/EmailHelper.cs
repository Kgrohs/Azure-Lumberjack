﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using Alertsense.Azure.Lumberjack.Controllers;
using AlertSense.Azure.Lumberjack.Contracts.Entities;
using log4net;
using OpenPop.Mime;
using OpenPop.Pop3;
using ServiceStack;

namespace Alertsense.Azure.Lumberjack.Helpers
{
    public static class EmailHelper
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(EmailHelper));


        //TODO: change the return type as needed
        public static List<SourcedAdoNetLog> GetLogEmails(int numEmails = 50, string email = "logsgohere1@gmail.com", string pass = "Logging123")
        {
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect("pop.gmail.com", 995, true);

                // Authenticate ourselves towards the server
                client.Authenticate(email, pass, AuthenticationMethod.UsernameAndPassword);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<SourcedAdoNetLog> allLogs = new List<SourcedAdoNetLog>();

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                { //TODO: i > messageCount - numEmails;     ** make sure that is greater than 0 though..
                    var message = client.GetMessage(i);
                    if (message == null) continue;

                    var plainText = message.FindFirstPlainTextVersion();
                    if (plainText == null) continue;

                    var body = plainText.GetBodyAsText();
                    try
                    {
                        var json = body.FromJson<AdoNetLog>();
                        SourcedAdoNetLog srcLog = new SourcedAdoNetLog
                        {
                            Source = "Email:\n" + email,
                            Date = json.Date,
                            Level = json.Level,
                            Logger = json.Logger,
                            Message = json.Message,
                            Exception = json.Exception,
                        };
                        allLogs.Add(srcLog);
                    }
                    catch (Exception e)
                    {
                        _log.Error(String.Format("Failed to parse an AdoNetLog out from the following email body:\n{0}", body), e);
                    }

                }
                return allLogs;
            }
        }

        public static string TrimAllWhitespace(this string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            foreach (char c in str)
            {
                if (!char.IsWhiteSpace(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}