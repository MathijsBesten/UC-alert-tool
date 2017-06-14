using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UC_alert_tool.Models;

namespace UC_alert_tool.Controllers
{
    [Authorize]
    public class TestersController : Controller
    {
        private alertDatabaseEntities db = new alertDatabaseEntities();

        public ActionResult emailtester()
        {
            ViewBag.smtpserverip = db.Settings.Single(s => s.Setting == "EmailServerIP").Value;
            ViewBag.smtpserverport = db.Settings.Single(s => s.Setting == "EmailServerPort").Value;
            return View();
        }
        [HttpPost]
        public ActionResult emailtester(email EmailToSend)
        {
            List<string> recipients = EmailToSend.Recipients[0].Split(',').ToList<string>(); // splitting the input on comma and creating multiple numbers
            EmailToSend.Recipients = recipients;
            Hangfire.BackgroundJob.Enqueue(() => Functions.Email.Sending.sendEmail(EmailToSend, true));
            TempData["showSuccess"] = true;
            TempData["showError"] = false;
            TempData["SuccessMessage"] = "Email wordt verzonden, controleer \"Admin tools > Achtergrondtaken\" om de voortgang te zien van de email verzending";
            return RedirectToAction("index","Home");
        }
        public ActionResult smstester()
        {
            ViewBag.smsgatewayip = db.Settings.Single(s => s.Setting == "SMSGatewayIP").Value; 
            ViewBag.smsgatewayusername = db.Settings.Single(s => s.Setting == "SMSGatewayUsername").Value;
            ViewBag.smsgatewaypassword = db.Settings.Single(s => s.Setting == "SMSGatewayPassword").Value;
            return View();
        }
        [HttpPost]
        public ActionResult smstester(SMS sms)
        {
            List<string> recipients = sms.Recipients[0].Split(',').ToList<string>(); // splitting the input on comma and creating multiple numbers
            sms.Recipients = recipients;
            Hangfire.BackgroundJob.Enqueue(() => Functions.SMS.Sending.sendSMSMessages(sms));
            TempData["showSuccess"] = true;
            TempData["showError"] = false;
            TempData["SuccessMessage"] = "sms(jes) wordt verzonden, controleer \"Admin tools > Achtergrondtaken\" om de voortgang te zien van de sms verzending";
            return RedirectToAction("index", "Home");
        }
    }
}