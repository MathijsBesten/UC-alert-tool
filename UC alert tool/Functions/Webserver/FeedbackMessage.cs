using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Functions.Webserver
{
    public class FeedbackMessage
    {
        public static void sendIamAliveEmail()
        {
            email mail = new email()
            {
                EmailBody = "UC alert tool is active and alive :)",
                EmailSubject = "UC alert tool - online",
                FromEmailAddress = Appsettings.Get.setting("EmailSendingMailAddress"),
                Recipients = Functions.Appsettings.Get.setting("keepAliveEmailAddresses").Split(';').ToList<string>(),
            };
            Email.Sending.sendEmail(mail,true); // if set to 'false' the mailgateway will not accept the mail and thinks it is spam
        }
    }
}