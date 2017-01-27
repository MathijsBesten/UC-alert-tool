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
        public static void sendEmailToOne(string fromEmailAddress, string toEmailAddress, string smtpServerAddress, string username, string password, string subject, string body)
        {
            MailMessage mail = new MailMessage(fromEmailAddress, toEmailAddress, subject, body);
            SmtpClient smtpClient = new SmtpClient(smtpServerAddress);
            smtpClient.Credentials = new NetworkCredential(username, password);
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