using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UC_alert_tool.Functions.Formatting
{
    public class SharepointFormatting
    {
        public static string getTextFromSharepointHTML(string original)
        {
            string originalString  = original.Replace("[", "").Replace("]", "");
            originalString = originalString.Substring(originalString.IndexOf(',') + 2);
            return originalString;
        }
    }
}