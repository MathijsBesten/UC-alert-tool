﻿using System;
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
        [Authorize]
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
            if (mailSuccesfullySend)
            {
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "al de emails zijn correct verzonden naar de e-mailserver";
            }
            else
            {
                TempData["showSuccess"] = false;
                TempData["showError"] = true;
                TempData["ErrorMessage"] = "Kon de mail niet afleveren aan de e-mailserver";
            }
            return RedirectToAction("index","Home");
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
            string probemRecipients = Functions.SMS.Sending.sendSMSToOne(sms);
            if (probemRecipients == null) // all the sms messages are delivered correcly
            {
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "Alle sms'jes zijn correct aangeboden aan de sms gateway";
                return RedirectToAction("index", "Home");
            }
            else
            {
                TempData["showSuccess"] = false;
                TempData["showError"] = true;
                TempData["ErrorMessage"] = "De volgende telefoonnummers konden niet worden aangeboden aan de sms gateway, " + probemRecipients;
                return RedirectToAction("index", "Home");
            }
        }
    }
}