using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Site
    {
        public string ID { get; set; }
        public string CampgroundID { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool Accessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool Utilities { get; set; }
    }
}
