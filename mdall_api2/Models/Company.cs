﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_api2.Models
{
    public class Company
    {
        public string company_id { get; set; }
        public string company_name { get; set; }
        public string addr_line_1 { get; set; }
        public string addr_line_2 { get; set; }
        public string addr_line_3 { get; set; }
        public string postal_code { get; set; }
        public string city { get; set; }
        public string country_cd { get; set; }
        public string region_cd { get; set; }
        public string company_status { get; set; }

    }
}
