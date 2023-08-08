using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendNotification.DTO
{
    public class TrackingInfo
    {
        public string codigo { get; set; }
        public string host { get; set; }
        public List<Event> eventos { get; set; }
        public double time { get; set; }
        public int quantidade { get; set; }
        public string servico { get; set; }
        public DateTime ultimo { get; set; }
    }
}
