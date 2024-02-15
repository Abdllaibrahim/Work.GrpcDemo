using Grpc.Core;
using Work.GrpcDemo.Server.Protos;

namespace Work.GrpcDemo.Server.Services
{
    public class TelemetryTrackingService : TrackingService.TrackingServiceBase
    {
        private readonly ILogger<TelemetryTrackingService> logger;

        public TelemetryTrackingService(ILogger<TelemetryTrackingService> logger)
        {
            this.logger = logger;
        }

        public override Task<TrackingResponse> SendMessage(TrackingMessage request, ServerCallContext context)
        {
            logger.LogInformation($"New Message: DeviceId: {request.DeviceId} " +
                $"Speed: {request.Speed} Device Names: {request.DeviceNames.Count}");

            return Task.FromResult(new TrackingResponse { Success = true });
        }


    }
}
