﻿using Hangfire;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.WebPages;
using UC_alert_tool.Models;

namespace UC_alert_tool.Controllers
{
    [Authorize]
    public class InstellingenController : Controller
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private alertDatabaseEntities db = new alertDatabaseEntities();


        // GET: Instellingen

        public ActionResult Index()
        {
            ViewBag.showSuccess = TempData["showSuccess"];
            ViewBag.showError = TempData["showError"];
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View();
        }

        public ActionResult logging()
        {
            ViewBag.currentLogLevel = ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level.ToString();
            string logPath = Server.MapPath("~/logs/");
            DirectoryInfo dirInfo = new DirectoryInfo(logPath);
            FileInfo[] files = dirInfo.GetFiles("*.log");
            List<string> list = new List<string>(files.Length);
            foreach (var item in files)
            {
                list.Add(item.Name);
            }
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult logging(string newLogLevel)
        {
            string currentLogLevel = ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level.ToString();
            switch (newLogLevel)
            {
                case "ALL":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.All;
                    break;

                case "DEBUG":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Debug;
                    break;

                case "INFO":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Info;
                    break;

                case "WARN":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Warn;
                    break;

                case "ERROR":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Error;
                    break;

                case "FATAL":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Fatal;
                    break;

                case "OFF":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = Level.Off;
                    break;

                default:
                    log.Error("There was a error while changing the log level - non of the cases applied.");
                    break;
            }
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
            log.Fatal("Changed log level from " + currentLogLevel + " to " + newLogLevel + "."); // It is set  to fatal - because otherwise you cannot see the message
            TempData["showSuccess"] = true;
            TempData["showError"] = false;
            TempData["SuccessMessage"] = "Log niveau is succesvol aangepast naar " + newLogLevel;
            return RedirectToAction("Index", "Instellingen");
        }
        public ActionResult DownloadLog (string filename)
        {
            if (Path.GetExtension(filename) == ".log")
            {
                string fullPath = Path.Combine(Server.MapPath("~/logs/"), filename);
                return File(fullPath, "text/log");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        public ActionResult smsgateway()
        {
            ViewBag.smsgatewayip = db.Settings.Single(s => s.Setting == "SMSGatewayIP").Value;
            ViewBag.smsgatewayusername = db.Settings.Single(s => s.Setting == "SMSGatewayUsername").Value;
            ViewBag.smsgatewaypassword = db.Settings.Single(s => s.Setting == "SMSGatewayPassword").Value;

            return View();
        }

        [HttpPost]
        public ActionResult smsgateway(string smsGatewayIP, string smsGatewayUsername, string smsGatewayPassword)
        {
            try
            {
                Functions.Appsettings.Edit.ChangeExistingValue("SMSGatewayIP", smsGatewayIP);
                Functions.Appsettings.Edit.ChangeExistingValue("SMSGatewayUsername",smsGatewayUsername);
                Functions.Appsettings.Edit.ChangeExistingValue("SMSGatewayPassword",smsGatewayPassword);
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "SMS server gegevens zijn succesvol aangepast";
                log.Info("User edited the sms gateway settings");

            }
            catch (Exception e)
            {
                log.Info("User tried to changed the sms gateway settings - error - " + e);
                TempData["ErrorMessage"] = "Kon de e-mailserver instellingen niet aanpassen, raadpleeg het logbestand voor meer informatie ";
            }
            return RedirectToAction("index", "Instellingen");
        }
        public ActionResult hangfire()
        {
            ViewBag.DatabaseSyncInterval = int.Parse(db.Settings.Single(s => s.Setting == "DatabaseUpdateTimer").Value);

            ViewBag.sharepointURL = db.Settings.Single(s => s.Setting == "SharepointURL").Value;
            ViewBag.sharepointusername = db.Settings.Single(s => s.Setting == "SharepointUsername").Value;
            ViewBag.sharepointpassword = db.Settings.Single(s => s.Setting == "SharepointPassword").Value;
            ViewBag.sharepointdomain = db.Settings.Single(s => s.Setting == "SharepointDomain").Value;
            return View();
        }
        [HttpPost]
        public ActionResult hangfire(int Interval, string url, string username, string password, string domain )
        {
            try
            {
                Functions.Appsettings.Edit.ChangeExistingValue("DatabaseUpdateTimer", Interval.ToString());
                Functions.Appsettings.Edit.ChangeExistingValue("SharepointURL", url.ToString());
                Functions.Appsettings.Edit.ChangeExistingValue("SharepointUsername", username.ToString());
                Functions.Appsettings.Edit.ChangeExistingValue("SharepointPassword", password.ToString());
                Functions.Appsettings.Edit.ChangeExistingValue("SharepointDomain", domain.ToString());

                if (ViewBag.DatabaseSyncInterval = int.Parse(db.Settings.Single(s => s.Setting == "DatabaseUpdateTimer").Value) != Interval)
                {
                    RecurringJob.AddOrUpdate("Database updaten", () => Functions.Helpdesk.Get.ClearDatabaseAndSync(), Cron.HourInterval(Interval));
                }
                log.Info("User changed the Database settings");
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "Database instellingen zijn succesvol aangepast";
            }
            catch (Exception e)
            {
                log.Error("User tried to change the Hangfire settings - failed - " + e);
                TempData["showSuccess"] = false;
                TempData["showError"] = true;
                TempData["ErrorMessage"] = "Kon de Hangfire instellingen niet aanpassen, raadpleeg het logbestand voor meer informatie ";
            }
            return RedirectToAction("index", "Instellingen");
        }

        public ActionResult emailserver()
        {
            ViewBag.emailserverip = db.Settings.Single(s => s.Setting == "EmailServerIP").Value;
            ViewBag.emailserverport = db.Settings.Single(s => s.Setting == "EmailServerPort").Value;
            ViewBag.emailsendaddress = db.Settings.Single(s => s.Setting == "EmailSendingMailAddress").Value;
            ViewBag.alertMailAdres = db.Settings.Single(s => s.Setting == "keepAliveEmailAddresses").Value;
            return View();
        }

        [HttpPost]
        public ActionResult emailserver(string emailServerIP, string emailServerPort, string keepaliveemailaddresses, string sendingEmailAddress)
        {
            try
            {
                Functions.Appsettings.Edit.ChangeExistingValue("EmailServerIP", emailServerIP);
                Functions.Appsettings.Edit.ChangeExistingValue("EmailServerPort",emailServerPort);
                Functions.Appsettings.Edit.ChangeExistingValue("EmailSendingMailAddress", sendingEmailAddress);
                Functions.Appsettings.Edit.ChangeExistingValue("keepAliveEmailAddresses", keepaliveemailaddresses);

                log.Info("User changed the mailserver settings");
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "SMTP server gegevens zijn succesvol aangepast";
            }
            catch (Exception e)
            {
                log.Error("User tried to change the mailserver settings - failed - " + e);
                TempData["showSuccess"] = false;
                TempData["showError"] = true;
                TempData["ErrorMessage"] = "Kon de e-mailserver instellingen niet aanpassen, raadpleeg het logbestand voor meer informatie ";
            }
            return RedirectToAction("index", "Instellingen");

        }
        public ActionResult emailtemplate()
        {
            Functions.Files.GeneralFiles.MakeSignatureFolderIfItNotExist();
            string path = db.Settings.Single(s => s.Setting == "SignaturePath").Value;
            ViewBag.imagePath = path;
            ViewBag.handtekeningtext = db.Settings.Single(s => s.Setting == "SignatureText").Value.Replace("<br />", Environment.NewLine);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult emailtemplate(HttpPostedFileBase file, string handtekeningText)
        {
            if (file != null && file.ContentType.Contains("png"))
            {
                Functions.Files.GeneralFiles.MakeSignatureFolderIfItNotExist();
                string path = db.Settings.Single(s => s.Setting == "SignaturePath").Value;
                string absolutePath = HostingEnvironment.MapPath(path);
                try
                {
                    file.SaveAs(absolutePath);
                    log.Info("User changed the signature picture withoud any problems");
                }
                catch (Exception e)
                {
                    log.Error("There was a error while trying to save the new signature picture");
                    log.Error(e);
                    throw;
                }
                Functions.Appsettings.Edit.ChangeExistingValue("signaturePath", path);
                log.Info("Signature image has been changed by the user");
                string htmlSignature = Functions.html_formatting.Replace.ReplaceEnters(handtekeningText);
                Functions.Appsettings.Edit.ChangeExistingValue("signaturetext", htmlSignature);
                log.Info("Signature text has been changed by the user");
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "De e-mailhandtekening is aangepast";
                return RedirectToAction("Index", "Instellingen");
            }
            else if (file == null) //this will catch if there is no picture uploaded
            {
                string htmlSignature = Functions.html_formatting.Replace.ReplaceEnters(handtekeningText);
                Functions.Appsettings.Edit.ChangeExistingValue("signaturetext", htmlSignature);
                log.Info("Signature text has been changed by the user");
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "De e-mailhandtekening is aangepast";
                return RedirectToAction("Index", "Instellingen");
            }
            else // this will trigger if the user manually select a file extention
            {
                log.Info("User tried to uploaded a file that was not a .PNG file - signature image not saved" );
                TempData["showSuccess"] = false;
                TempData["showError"] = true;
                TempData["ErrorMessage"] = "Je kan enkel een .png bestand uploaden voor de handtekening - raadpleeg de ontwikkelaar als dit probleem blijft ontstaan";
                return RedirectToAction("Index", "Instellingen");

            }
        }
        public ActionResult homepage()
        {
            ViewBag.ActiveTitle = Functions.Appsettings.Get.setting("activeIssuesTitle");
            ViewBag.PlannedTitle = Functions.Appsettings.Get.setting("plannedIssuesTitle");
            ViewBag.OldTitle = Functions.Appsettings.Get.setting("solvedIssuesTitle");
            ViewBag.OldMessageTime = Functions.Appsettings.Get.setting("OldMessageTime");
            ViewBag.homepagetext = Functions.Appsettings.Get.setting("homepageText");
            ViewBag.homepagetitle = Functions.Appsettings.Get.setting("homepageTitle");
            return View();
        }
        [HttpPost]
        public ActionResult homepage(string ActiveTitle, string PlannedTitle, string OldTitle , string OldMessageTime, string homepagetitle, string homepagetext)
        {
            try
            {
                Functions.Appsettings.Edit.ChangeExistingValue("activeIssuesTitle", ActiveTitle);
                Functions.Appsettings.Edit.ChangeExistingValue("plannedIssuesTitle", PlannedTitle);
                Functions.Appsettings.Edit.ChangeExistingValue("solvedIssuesTitle", OldTitle);
                Functions.Appsettings.Edit.ChangeExistingValue("OldMessageTime", OldMessageTime);
                Functions.Appsettings.Edit.ChangeExistingValue("homepageTitle", homepagetitle);
                Functions.Appsettings.Edit.ChangeExistingValue("homepageText",homepagetext);

                log.Info("Homepage column text has been changed by the user");
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "De waardes op de homepagina zijn aangepast";
                return RedirectToAction("Index", "Instellingen");


            }
            catch (Exception e)
            {
                log.Info("User tried to change homepage text - error: " + e.Message);
                TempData["showSuccess"] = false;
                TempData["showError"] = true;
                TempData["ErrorMessage"] = "Er was een probleem bij het opslaan van de nieuwe homepagina teskten - raadpleeg het logbestand voor meer informatie";
                return RedirectToAction("Index", "Instellingen");
            }
        }
        public ActionResult contact()
        {
            ViewBag.contactdetails = Functions.Appsettings.Get.setting("contactDetails");
            return View();
        }
        [HttpPost]
        public ActionResult contact(string contactdetails)
        {
            try
            {
                Functions.Appsettings.Edit.ChangeExistingValue("contactDetails", contactdetails);
                log.Info("Homepage column text has been changed by the user");
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "De contactgegevens zijn aangepast";
                return RedirectToAction("Index", "Instellingen");


            }
            catch (Exception e)
            {
                log.Info("User tried to change homepage text - error: " + e.Message);
                TempData["showSuccess"] = false;
                TempData["showError"] = true;
                TempData["ErrorMessage"] = "Er was een probleem bij het opslaan van de nieuwe homepagina teskten - raadpleeg het logbestand voor meer informatie";
                return RedirectToAction("Index", "Instellingen");
            }
        }
    }
}