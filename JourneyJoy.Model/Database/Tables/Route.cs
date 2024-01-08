namespace JourneyJoy.Model.Database.Tables
{
    public record Route
    {
        public Guid Id { get; set; }
        public Trip Trip { get; set; }
        public Guid TripId { get; set; }
        public int StartDay { get; set; }
        public Guid StartPointAttractionId { get; set; }
        public string? SerializedAttractionsIds {  get; set; }

        public int NumberOfDays { get; set; }
    }

}
