namespace MeuContexto
{
    public class PortalException : Exception
    {
        public new string Message { get; set; }
        public bool Sucess { get; set; }
        public PortalException(string message, bool success = false)
        {
            Message = message;
            Sucess = success;
        }
    }
}
