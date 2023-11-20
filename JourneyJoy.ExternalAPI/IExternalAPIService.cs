namespace JourneyJoy.ExternalAPI
{
    public interface IExternalApiService
    {
        public TripAdvisorAPI TripAdvisorAPI { get; }
        public GoogleMapsAPI GoogleMapsAPI { get; }
    }
}