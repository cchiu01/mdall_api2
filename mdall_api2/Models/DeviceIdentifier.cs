using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdall_api2.Models
{
    public class DeviceIdentifier
    {
        public string original_licence_no { get; set; }
        public string device_id { get; set; }
        public string first_licence_dt { get; set; }
        public string end_date { get; set; }
        public string device_identifier { get; set; }
    }
}
