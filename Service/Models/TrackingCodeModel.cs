namespace Service.Models
{
    public class TrackingCodeModel
    {
        public int TrackingCodeId { get; set; }
        public string Code { get; set; }
        public int SubPersonId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public TrackingCodeStatus Status { get; set; }
        public DateTime NextSearch { get; set; }
        public int NumberOfTries { get; set; }

        public TrackingCodeModel(string code, int subPersonId)
        {
            Code = code;
            Status = TrackingCodeStatus.Active;
            CreationDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            SubPersonId = subPersonId;
            NextSearch = DateTime.Now;
        }
        public TrackingCodeModel()
        {

        }
    }
}

