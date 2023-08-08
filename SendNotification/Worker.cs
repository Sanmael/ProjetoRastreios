using Microsoft.Extensions.DependencyInjection;
using ProjetoJqueryEstudos.UOW;
using SendNotification.Service;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace SendNotification
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;


        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    new SendEmail(unitOfWork).SendMailAsync();

                    await Task.Delay(20000, stoppingToken);
                }

            }
        }
    }
}