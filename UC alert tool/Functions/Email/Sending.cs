using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Email
{
    public class Sending
    {
        public static void sendEmail(email Email)
        {
            MailMessage mail = new MailMessage(Email.FromEmailAddress, Email.FromEmailAddress, Email.EmailSubject, Email.EmailBody);
            foreach (string recipientEmailAddress in Email.Recipients)
            {
                mail.Bcc.Add(recipientEmailAddress);
            }
            string host = Email.SMTPServerURL.Split(':')[0];
            int port = Int32.Parse(Email.SMTPServerURL.Split(':')[1]);
            SmtpClient smtpClient = new SmtpClient(host);
            smtpClient.Credentials = new NetworkCredential(Email.SMTPUsername, "");
            smtpClient.Port = port;
            smtpClient.EnableSsl = false;
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}