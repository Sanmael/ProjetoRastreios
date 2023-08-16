using MeuContexto.UOW;
using Microsoft.Extensions.DependencyInjection;
using SendNotification.Service;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace SendNotification
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    await new SendEmail(unitOfWork,_configuration).GetCodesAsync();

                    await Task.Delay(20000, stoppingToken);
                }

            }
        }
    }
}