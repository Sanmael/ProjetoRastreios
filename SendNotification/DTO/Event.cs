using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendNotification.DTO
{

    public class Event
    {
        public string data { get; set; }
        public string hora { get; set; }
        public string local { get; set; }
        public string status { get; set; }
        public List<string> subStatus { get; set; }
    }

}
