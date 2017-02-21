using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace UC_alert_tool.Controllers
{
    public class InstellingenController : Controller
    {

        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Instellingen
        public ActionResult Index()
        {
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
        [ValidateAntiForgeryToken]
        public ActionResult smsgateway(string smsGatewayIP, string smsGatewayUsername, string smsGatewayPassword)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings["SMSGatewayIP"].Value = smsGatewayIP;
            config.AppSettings.Settings["SMSGatewayUsername"].Value = smsGatewayUsername;
            config.AppSettings.Settings["SMSGatewayPassword"].Value = smsGatewayPassword;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            return RedirectToAction("Index", "home");
        }
        public ActionResult emailserver()
        {
            ViewBag.emailserverip = WebConfigurationManager.AppSettings["EmailServerIP"];
            ViewBag.emailserverport = WebConfigurationManager.AppSettings["EmailServerPort"];
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult emailserver(string emailServerIP, string emailServerPort)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            config.AppSettings.Settings["EmailServerIP"].Value = emailServerIP;
            config.AppSettings.Settings["EmailServerPort"].Value = emailServerPort;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            return RedirectToAction("Index", "home");
        }
    }
}