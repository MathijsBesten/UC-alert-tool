using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using UC_alert_tool.Models;
using log4net;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net.Mime;
using System.Web.Hosting;

namespace UC_alert_tool.Functions.Email
{
    public class Sending
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static alertDatabaseEntities db = new alertDatabaseEntities();

        public static bool sendEmail(email Email, bool useSignature )
        {
            MailMessage mail = new MailMessage(Email.FromEmailAddress, Email.FromEmailAddress);
            Email.EmailBody = Email.EmailBody.Replace(Environment.NewLine, @"<br>");
            mail.Subject = Email.EmailSubject;
            mail.IsBodyHtml = true;
            if (useSignature)
            {
                mail.AlternateViews.Add(getEmbeddedImage(Email.EmailBody));
            }
            else
            {
                mail.Body = Email.EmailBody;
            }
            foreach (string recipientEmailAddress in Email.Recipients)
            {
                if (new EmailAddressAttribute().IsValid(recipientEmailAddress))
                {
                    mail.Bcc.Add(recipientEmailAddress);
                }
                else
                {
                    log.Info("Not valid email address detected when sending email- " + recipientEmailAddress);
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
        private static AlternateView getEmbeddedImage(string stringToSend)
        {
            LinkedResource inline = new LinkedResource(HostingEnvironment.ApplicationPhysicalPath + ((db.Settings.Single(s => s.Setting == "SignaturePath").Value)));
            inline.ContentId = Guid.NewGuid().ToString();
            //combine body with image and signature
            string emailBody = "";
            emailBody += "<html><body>";
            emailBody += stringToSend;
            emailBody += "<br> <br>"; // addes a whiteline for the signature
            emailBody += db.Settings.Single(s => s.Setting == "SignatureText").Value;
            emailBody += "<br>";
            emailBody += @"<img src='cid:" + inline.ContentId + @"'/>";
            emailBody += "</body></html>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(emailBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);
            return alternateView;
        }
    }
}