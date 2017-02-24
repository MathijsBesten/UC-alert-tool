using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Functions.Time
{
    public class GetTime
    {
        public static string GetGreetingsmessageBasedOnTimeOfDay()
        {
            DateTime currentTime = DateTime.Now;
            string greetingMessage = "";
            if (currentTime.Hour < 12)
            {
                greetingMessage = "Goedemorgen";
            }
            else if (currentTime.Hour < 18)
            {
                greetingMessage = "Goedemiddag";
            }
            else
            {
                greetingMessage = "Goedenavond";
            }
            return greetingMessage;
        }
    }
}