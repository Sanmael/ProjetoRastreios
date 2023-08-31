namespace Domain.Interfaces
{
    public interface ILoggerRepository
    {
        public void LogError(Exception ex, string message);
    }
}
