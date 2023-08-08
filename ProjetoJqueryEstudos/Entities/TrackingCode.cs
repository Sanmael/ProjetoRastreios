namespace ProjetoJqueryEstudos.Entities
{
    public class TrackingCode
    {
        public int TrackingCodeId { get; set; }
        public string Code { get; set; }
        public int SubPersonId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public TrackingCodeStatus Status { get; set; }
        public DateTime NextSearch {get; set; }

        public TrackingCode(string code, int subPersonId)
        {           
            Code = code;
            Status = TrackingCodeStatus.Active;
            CreationDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            SubPersonId = subPersonId;
        }
    }
}
