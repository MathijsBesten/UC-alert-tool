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
        public static void sendEmailToOne(email Email)
        {
            MailMessage mail = new MailMessage(Email.FromEmailAddress, Email.FromEmailAddress, Email.EmailSubject, Email.EmailBody);
            foreach (string recipientEmailAddress in Email.Recipients)
            {
                mail.Bcc.Add(recipientEmailAddress);
            }
            SmtpClient smtpClient = new SmtpClient(Email.SMTPServerURL);
            smtpClient.Credentials = new NetworkCredential(Email.SMTPUsername, Email.SMTPPassword);
            smtpClient.EnableSsl = true;
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); ;
            }
        }
    }
}