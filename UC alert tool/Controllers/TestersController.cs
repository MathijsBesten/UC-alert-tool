using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UC_alert_tool.Models;

namespace UC_alert_tool.Controllers
{
    [Authorize]
    public class TestersController : Controller
    { 
        public ActionResult emailtester()
        {
            return View();
        }
        [HttpPost]
        public ActionResult emailtester(email EmailToSend)
        {
            List<string> recipients = EmailToSend.Recipients[0].Split(',').ToList<string>(); // splitting the input on comma and creating multiple numbers
            EmailToSend.Recipients = recipients;
            bool mailSuccesfullySend = Functions.Email.Sending.sendEmail(EmailToSend);

            return View();
        }
        public ActionResult smstester()
        {
            return View();
        }
        [HttpPost]
        public ActionResult smstester(SMS sms)
        {
            List<string> recipients = sms.Recipients[0].Split(',').ToList<string>(); // splitting the input on comma and creating multiple numbers
            sms.Recipients = recipients;
            Functions.SMS.Sending.sendSMSToOne(sms);
            return View();
        }
    }
}