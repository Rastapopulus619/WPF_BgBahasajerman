namespace Bgb_DataAccessLibrary.Contracts
{
    public interface ILoggerService
    {
        void Log(string message);
        void LogException(Exception ex);
    }
}