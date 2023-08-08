namespace ProjetoJqueryEstudos.Entities
{
    public class TrackingCodeEvents
    {
        public int TrackingCodeEventsId { get; set; }
        public int TrakingCodeId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; } 
        public string Local { get; set; }
    }   
}
