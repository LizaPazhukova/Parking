// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.
using CoolParking.BL.Interfaces;
using System;
using System.IO;

namespace CoolParking.BL.Services
{
    public class LogService: ILogService
    {
        private readonly string _logFilePath;
        public LogService(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public string LogPath { get { return _logFilePath; } }

        public string Read()
        {
            if (!File.Exists(_logFilePath))
            {
                throw new InvalidOperationException();
            }

            using (var file = new StreamReader(_logFilePath))
            {
                return file.ReadToEnd();
            }
        }

        public void Write(string logInfo)
        {
            using (var file = new StreamWriter(_logFilePath, true))
            {
                file.WriteLine(logInfo);
            }
        }
    }
}
