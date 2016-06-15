using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var results = Query.GetSearchResults("Flying Colors");
            using (var ctx = new ResultsStorage())
            {
                foreach (var result in results)
                {
                    ctx.Results.Add(result);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
