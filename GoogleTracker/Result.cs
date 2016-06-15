using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleTracker
{
    public class Result
    {
        public Result() { }

        [Key]
        public int idx { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int searchPageNo { get; set; }
        public SearchInstance si { get; set; }
    }
}
