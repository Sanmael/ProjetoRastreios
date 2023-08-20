using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class TrackingCodeEvents
    {
        [Key] // Aplicando o KeyAttribute à propriedade

        public int TrackingCodeEventsId { get; set; }
        public int TrakingCodeId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public string Local { get; set; }
    }
}
