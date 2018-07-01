using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Reservation
    {
        public string ID { get; set; }
        public string SiteID { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
