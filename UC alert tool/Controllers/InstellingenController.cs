using log4net;
using log4net.Core;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
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
            return View();
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
            log.Fatal("Changed log level from " + currentLogLevel + " to " + newLogLevel + ".");
            return RedirectToAction("Index", "home");
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

            }
            catch (Exception e)
            {
                log.Info("User tried to changed the sms gateway settings - error - " + e);
                TempData["ErrorMessage"] = "Kon de e-mailserver instellingen niet aanpassen, raadpleeg het logbestand voor meer informatie ";
            }
            return RedirectToAction("index", "Home");
        }

        public ActionResult emailserver()
        {
            ViewBag.emailserverip = db.Settings.Single(s => s.Setting == "EmailServerIP").Value;
            ViewBag.emailserverport = db.Settings.Single(s => s.Setting == "EmailServerPort").Value;
            return View();
        }

        [HttpPost]
        public ActionResult emailserver(string emailServerIP, string emailServerPort)
        {
            try
            {
                Functions.Appsettings.Edit.ChangeExistingValue( "EmailServerIP", emailServerIP);
                Functions.Appsettings.Edit.ChangeExistingValue("EmailServerPort",emailServerPort);
                log.Info("User changed the mailserver settings");
                TempData["showSuccess"] = true;
                TempData["showError"] = false;
                TempData["SuccessMessage"] = "SMTP server gegevens zijn succesvol aangepast";
            }
            catch (Exception e)
            {
                log.Error("User tried to change the mailserver settings- failed - " + e);
                TempData["showSuccess"] = false;
                TempData["showError"] = true;
                TempData["ErrorMessage"] = "Kon de e-mailserver instellingen niet aanpassen, raadpleeg het logbestand voor meer informatie ";
            }
            return RedirectToAction("index", "Home");

        }

        public ActionResult emailtemplate()
        {
            ViewBag.handtekeningtext = db.Settings.Single(s => s.Setting == "SignatureText").Value;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult emailtemplate(HttpPostedFileBase file, string handtekeningText)
        {
            if (file != null)
            {
                Functions.Files.GeneralFiles.MakeSignatureFolderIfItNotExist();
                string path = db.Settings.Single(s => s.Setting == "SignaturePath").Value;
                string absolutePath = HostingEnvironment.MapPath(path);
                file.SaveAs(absolutePath);
                Functions.Appsettings.Edit.ChangeExistingValue("signaturePath", path);
                log.Info("Signature has been changed by the user");
            }
            string htmlSignature = handtekeningText.Replace(@"\r\n", @"<br />");
            Functions.Appsettings.Edit.ChangeExistingValue("signaturetext", htmlSignature);
            return RedirectToAction("Index", "home");
        }
    }
}