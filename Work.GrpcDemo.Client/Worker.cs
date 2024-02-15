using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Work.GrpcDemo.Server.Protos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Work.GrpcDemo.Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int deviceId;
        private TrackingService.TrackingServiceClient _client;

        private TrackingService.TrackingServiceClient Client
        {
            get
            {
                if (_client == null)
                {
                    var channel = GrpcChannel.ForAddress("https://localhost:5001");

                    _client = new TrackingService.TrackingServiceClient(channel);
                }

                return _client;
            }
        }

        public Worker(ILogger<Worker> logger, int deviceId)
        {
            _logger = logger;
            this.deviceId = deviceId;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Random random = new Random();

             while (!stoppingToken.IsCancellationRequested)
            {
                await SendMessage(random);

                await Task.Delay(1000, stoppingToken);
            }

        }

        private async Task SendMessage(Random random)
        {
            var request = new TrackingMessage
            {
                DeviceId = deviceId,
                Speed = random.Next(0, 220),
                Stamp = Timestamp.FromDateTime(DateTime.UtcNow)
            };

            request.DeviceNames.Add("HP");
            request.DeviceNames.Add("Lenovo");

            var response = await Client.SendMessageAsync(request);

            _logger.LogInformation($"Response: {response.Success}");
        }
    }
}
