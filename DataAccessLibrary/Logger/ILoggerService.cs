
namespace Bgb_DataAccessLibrary.Logger
{
    public interface ILoggerService
    {
        void Log(string message);
        void LogException(Exception ex);
    }
}