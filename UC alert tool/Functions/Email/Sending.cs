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

        public static bool sendEmail(email Email, bool useSignature) 
        {
            //generate mail
            MailMessage mail = new MailMessage(Email.FromEmailAddress, Email.FromEmailAddress);
            Email.EmailBody = Email.EmailBody.Replace(Environment.NewLine, @"<br>");
            mail.Subject = Email.EmailSubject;
            if (useSignature)
            {
                mail.AlternateViews.Add(getEmbeddedImage(Email.EmailBody));
                mail.IsBodyHtml = true;
            }
            else
            {
                mail.Body = Email.EmailBody;
                mail.IsBodyHtml = false;
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


            // mailserver settings
            string host = Appsettings.Get.setting("EmailServerIP");
            if (!string.IsNullOrEmpty(Email.SMTPServerURL))
            {
                host = Email.SMTPServerURL.Split(':')[0];
            }
            int port = int.Parse(Appsettings.Get.setting("EmailServerPort"));
            if (!string.IsNullOrEmpty(Email.SMTPServerURL))
            {
                port = int.Parse(Email.SMTPServerURL.Split(':')[1]);
            }
            string fromEmailAddress = Appsettings.Get.setting("EmailSendingMailAddress");
            if (!string.IsNullOrEmpty(Email.FromEmailAddress))
            {
                fromEmailAddress = Email.FromEmailAddress;
            }
            SmtpClient smtpClient = new SmtpClient(host);
            smtpClient.Credentials = new NetworkCredential(fromEmailAddress, "");
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
                throw new InvalidOperationException("Failed to send email - " + ex);
            }
        }
        private static AlternateView getEmbeddedImage(string stringToSend) // a email template
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