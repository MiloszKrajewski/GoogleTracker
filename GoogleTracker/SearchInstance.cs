using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleTracker
{
    public class SearchInstance
    {
        public SearchInstance() { }

        [Key]
        public int idx { get; set; }
        public DateTime retrieved { get; set; }
        public string query { get; set; }
        public ulong count { get; set; }
    }
}
