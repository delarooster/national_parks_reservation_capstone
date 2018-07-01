using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Campground
    {
        public string ID { get; set; }
        public string ParkID { get; set; }
        public string Name { get; set; }
        public int OpenFrom { get; set; }
        public int OpenTo { get; set; }
        public double DailyFee { get; set; }
    }
}
