using log4net;
using log4net.Core;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace UC_alert_tool.Controllers
{
    [Authorize]
    public class InstellingenController : Controller
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool showSuccess = false;
        public bool showError = false;
        public string SuccessMessage = null;
        public string ErrorMessage = null;

        // GET: Instellingen

        public ActionResult Index()
        {
            ViewBag.showSuccess = showSuccess;
            ViewBag.showError = showError;
            ViewBag.SuccessMessage = SuccessMessage;
            ViewBag.ErrorMessage = ErrorMessage;

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
            ViewBag.smsgatewayip = WebConfigurationManager.AppSettings["SMSGatewayIP"];
            ViewBag.smsgatewayusername = WebConfigurationManager.AppSettings["SMSGatewayUsername"];
            ViewBag.smsgatewaypassword = WebConfigurationManager.AppSettings["SMSGatewayPassword"];

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
                showSuccess = true;
                showError = false;
                SuccessMessage = "SMS server gegevens zijn succesvol aangepast";

            }
            catch (Exception e)
            {
                log.Info("User tried to changed the sms gateway settings - error - " + e);
                showSuccess = false;
                showError = true;
                ErrorMessage = "Kon de e-mailserver instellingen niet aanpassen, raadpleeg het logbestand voor meer informatie ";
            }

            return RedirectToAction("index", "Home");
        }

        public ActionResult emailserver()
        {
            ViewBag.emailserverip = WebConfigurationManager.AppSettings["EmailServerIP"];
            ViewBag.emailserverport = WebConfigurationManager.AppSettings["EmailServerPort"];
            return View();
        }

        [HttpPost]
        public ActionResult emailserver(string emailServerIP, string emailServerPort)
        {
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings["EmailServerIP"].Value = emailServerIP;
                config.AppSettings.Settings["EmailServerPort"].Value = emailServerPort;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                log.Info("User changed the mailserver settings");
                showSuccess = true;
                showError = false;
                SuccessMessage = "SMTP server gegevens zijn succesvol aangepast";
            }
            catch (Exception e)
            {
                log.Error("User tried to change the mailserver settings- failed - " + e);
                showSuccess = false;
                showError = true;
                ErrorMessage = "Kon de e-mailserver instellingen niet aanpassen, raadpleeg het logbestand voor meer informatie ";
            }
            return RedirectToAction("index", "Home");

        }

        public ActionResult emailtemplate()
        {
            ViewBag.handtekeningtext = WebConfigurationManager.AppSettings["SignatureText"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult emailtemplate(HttpPostedFileBase file, string handtekeningText)
        {
            if (file != null)
            {
                Functions.Files.GeneralFiles.MakeSignatureFolderIfItNotExist();
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ConfigurationManager.AppSettings["FileDirectory"], ConfigurationManager.AppSettings["SignatureDirectory"],  ConfigurationManager.AppSettings["SignatureImage"]);
                file.SaveAs(path);
                log.Info("Signature has been changed by the user");
            }

            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings["signaturetext"].Value = handtekeningText;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");


            return RedirectToAction("Index", "home");
        }
    }
}