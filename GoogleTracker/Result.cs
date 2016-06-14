using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleTracker
{
    public class Result
    {
        public Result() { }

        public string url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int searchPageNo { get; set; }
        public SearchInstance si { get; set; }
    }
}
