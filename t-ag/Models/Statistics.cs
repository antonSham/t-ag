using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t_ag.Models
{
    public class Statistics
    {
        public Statistics()
        {
            orders = 0;
            total = 0;
            canceled = 0;
        }

        public string id { get; set; }
        public int? orders { get; set; }
        public int? total { get; set; }
        public int? avg { get; set; }
        public int? canceled { get; set; }
    }
}
