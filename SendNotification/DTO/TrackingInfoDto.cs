using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendNotification.DTO
{
    public class TrackingInfoDto
    {
        public string codigo { get; set; }
        public List<EventDto> eventos { get; set; }
        public double time { get; set; }
    }
}
