namespace ProjetoJqueryEstudos.Entities
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ErrorLog(DateTime timestamp, string message, string stackTrace)
        {            
            Timestamp = timestamp;
            Message = message;
            StackTrace = stackTrace;
        }
    }
}
