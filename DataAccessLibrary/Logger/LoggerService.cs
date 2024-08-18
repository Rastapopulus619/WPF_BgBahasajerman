
namespace Bgb_DataAccessLibrary.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly string _logDirectory;

        public LoggerService(string logDirectory)
        {
            _logDirectory = logDirectory;

            // Ensure the directory exists
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public void Log(string message)
        {
            string logFilePath = Path.Combine(_logDirectory, "log.txt");
            File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
        }

        public void LogException(Exception ex)
        {
            Log(ex.Message);
        }
    }
}