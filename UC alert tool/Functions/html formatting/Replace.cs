﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace UC_alert_tool.Functions.html_formatting
{
    public class Replace
    {
        public static string ReplaceEnters (string stringToFormat)
        {
            return Regex.Replace(stringToFormat, @"\r\n?|\n", "<br />");
        }
    }
}