using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t_ag.Models
{
    public class User
    {
        public int id { set; get; }
        public String role { set; get; }
        public String login { set; get; }
        public String password { set; get; }

        public String toString()
        {
            return this.role + '\t' + this.login + '\t' + this.password;
        }
    }
}
