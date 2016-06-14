using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleTracker
{
    public class SearchInstance
    {
        public SearchInstance() { }

        public DateTime retrieved { get; set; }
        public string query { get; set; }
        public ulong count { get; set; }
    }
}
