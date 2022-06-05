using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_api2.Models
{
    public class Licence
    {
        public string original_licence_no { get; set; }
        public string licence_status { get; set; }
        public string appl_risk_class { get; set; }
        public string licence_name { get; set; }
        public string first_licence_status_dt { get; set; }
        public string last_refresh_dt { get; set; }
        public string end_date { get; set; }
        public string licence_type_cd { get; set; }
    }
}
