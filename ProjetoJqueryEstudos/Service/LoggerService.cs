namespace ProjetoJqueryEstudos.Service
{
    using Microsoft.EntityFrameworkCore;
    using NuGet.Packaging.Signing;
    using ProjetoJqueryEstudos.Context;
    using ProjetoJqueryEstudos.Entities;
    using ProjetoJqueryEstudos.Interfaces;
    using Serilog;
    using Serilog.Events;
    using System;
    using System.Diagnostics;

    public class LoggerService : ILoggerService
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
