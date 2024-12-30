namespace Bgb_DataAccessLibrary.Contracts.IServices.ILogging
{
    public interface ILoggerService
    {
        void Log(string message);
        void LogException(Exception ex);
    }
}