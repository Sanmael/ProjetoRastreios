namespace MeuContexto.Service
{
    using Entities;
    using Entities.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using ProjetoJqueryEstudos.Interfaces;
    using Serilog;
    using System;
    using System.Diagnostics;

    public class LoggerService : ILoggerRepository
    {
        private readonly ILogger _logger;

        private readonly IRepository _repository;

        public LoggerService(IRepository repository)
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            _repository = repository;
        }

        public void LogError(Exception ex, string message)
        {
            ErrorLog errorLog = new ErrorLog(DateTime.Now, message, ex.ToString());

            _repository.SaveEntity(errorLog);
        }
    }

}
