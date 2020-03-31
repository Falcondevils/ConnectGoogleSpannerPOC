using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectGoogleSpannerPOC.Models
{
    public class Offering
    {
        public string OfferingId { get; set; }
        public int CourseDurationInYears { get; set; }
        public string OfferingName { get; set; }
        public int TotalCourseFee { get; set; }

    }
}
