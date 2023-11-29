namespace GettingStarted
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using Microsoft.Extensions.Hosting;

    public class Worker : BackgroundService
    {
        readonly IBus _bus;
        readonly ILogger<Worker> _logger;


        public Worker(IBus bus, ILogger<Worker> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _bus.Publish(new GettingStarted { Value = $"The time is {DateTimeOffset.Now}" }, stoppingToken);
                _logger.LogInformation("Message sent to bus.");

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
