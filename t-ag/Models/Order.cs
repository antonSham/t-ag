using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t_ag.Models
{
    public class Order
    {
        public int id { set; get; }
        public Tour tour { set; get; }
        public int price { set; get; }
        public List<Participant> participants { set; get; }
        public User customer { set; get; }
        public User employee { set; get; }
        public int amount { set; get; }
    }
}
