namespace JourneyJoy.Backend.Options
{
    public record AppOptions
    {
        public string DatabaseConnectionString { get; set; } = null!;
        public string JwtKey { get; set; } = null!;
        public string JwtIssuer { get; set; } = null!;
        public string TripAdvisorAPIKey { get; set; } = null!;
        public bool IsTripAdvisorAPIEnabled { get; set; }
        public string GoogleAPIKey { get; set; } = null!;
        public bool IsGoogleAPIEnabled { get; set; }

    }
}
