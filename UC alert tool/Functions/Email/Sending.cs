using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using UC_alert_tool.Models;
using log4net;
using System.ComponentModel.DataAnnotations;

namespace UC_alert_tool.Functions.Email
{
    public class Sending
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool sendEmail(email Email)
        {
            MailMessage mail = new MailMessage(Email.FromEmailAddress, Email.FromEmailAddress, Email.EmailSubject, Email.EmailBody);
            foreach (string recipientEmailAddress in Email.Recipients)
            {
                if (new EmailAddressAttribute().IsValid(recipientEmailAddress))
                {
                    mail.Bcc.Add(recipientEmailAddress);
                }
                else
                {
                    log.Error("Not valid email address detected when sending email- " + recipientEmailAddress);
                }
            }
            string host = Email.SMTPServerURL.Split(':')[0];
            int port;
            if (Email.SMTPServerURL.Split(':').Count() == 1 ) // if only a url without a port is entered
            {
                port = 587;
            }
            else
            {
                port = Int32.Parse(Email.SMTPServerURL.Split(':')[1]);
            }
            SmtpClient smtpClient = new SmtpClient(host);
            smtpClient.Credentials = new NetworkCredential(Email.SMTPUsername, "");
            smtpClient.Port = port;
            smtpClient.EnableSsl = false;
            try
            {
                smtpClient.Send(mail);
                log.Info("email(s) are send succesfully to the mail gateway");
                return true;
            }
            catch (Exception ex)
            {
                log.Info("Error while sending the email(s) to the mail gateway - " + ex);
                return false;
                throw ex;
            }
        }
    }
}