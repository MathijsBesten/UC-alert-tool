using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.Net.Http;

namespace UC_alert_tool.Functions.SMS
{
    public class Sending
    {
        public static void sendSMSToOne(Models.SMS sms)
        {
            string url = "http://{0}/cgi-bin/sms_send?username={1}&password={2}&number={3}&text={4}";
            using (WebClient client = new WebClient())
            {
                int count = 0; 
                foreach (string recipient in sms.Recipients)
                {
                    string completeURL = string.Format(url, sms.server, sms.username, sms.password, sms.Recipients[count], sms.text);
                    var respone = client.DownloadString(completeURL);
                    count++;
                }
                client.Dispose();
            }
        }
    }
}