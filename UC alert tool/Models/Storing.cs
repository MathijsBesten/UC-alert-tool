﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UC_alert_tool.Models;

namespace UC_alert_tool.Models
{
    public class Storing
    {
        public int ID { get; set; }
        public int productID { get; set; }
        public string Titel { get; set; }
        public string inhoud { get; set; }
        public int eigenaarID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? begindatum { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? einddatum { get; set; }
        public bool isGesloten { get; set; }


    }
}