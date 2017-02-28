using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.Net.Http;
using log4net;

namespace UC_alert_tool.Functions.SMS
{
    public class Sending
    {
        private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string sendSMSToOne(Models.SMS sms) // returns a string of Recipients that had a problem while sending - return null if all Recipients where send correcly
        {
            string allRecipientsInString = "";
            foreach (string number in sms.Recipients){allRecipientsInString += (" " + number);}
            log.Info("Sending sms messages to the following numbers, " + allRecipientsInString);
            List<string> notInformedRecipients = new List<string>();
            string url = "http://{0}/cgi-bin/sms_send?username={1}&password={2}&number={3}&text={4}";
            using (WebClient client = new WebClient())
            {
                int count = 0; 
                foreach (string recipient in sms.Recipients)
                {
                    try
                    {
                        string completeURL = string.Format(url, sms.server, sms.username, sms.password, sms.Recipients[count], sms.text);
                        var respone = client.DownloadString(completeURL);
                        if (!respone.Contains("OK"))// there are no other responses that contains the letters "OK" 
                        {
                            log.Error("SMS not send to " + sms.Recipients[count] + " - response from sms server " + respone);
                            notInformedRecipients.Add(sms.Recipients[count]);
                        }
                    }
                    catch (Exception e)
                    {
                        log.Error("SMS not send to " + sms.Recipients[count] + " - error " + e);
                        notInformedRecipients.Add(sms.Recipients[count]);
                    }

                    count++;
                }
                client.Dispose();
            }
            if (notInformedRecipients.Count == 0)
            {
                log.Info("All sms messages are correcly delivered to the sms gateway");
                return null;
            }
            else
            {
                string notInformedRecipientsInString = "";
                foreach (string number in notInformedRecipients) { notInformedRecipientsInString += (" " + number); }
                log.Error("These Recipients did not receive a sms - " + notInformedRecipientsInString);
                return notInformedRecipientsInString;
            }
        }
    }
}