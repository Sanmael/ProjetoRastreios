namespace Entities.Interfaces
{
    public interface ILoggerRepository
    {
        public void LogError(Exception ex, string message);
    }
}
