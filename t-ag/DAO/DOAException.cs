using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t_ag.DAO
{
    public class DOAException : Exception
    {
        public DOAException(string message, Exception innerException) : base(message, innerException) { }
    }
}
