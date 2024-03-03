namespace ChallengeApp.CommonLayer.Model
{
    public class ReadAvgAgeRecordRequest
    {
        public string Team { get; set; }
    }

    public class ReadAvgAgeRecordResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public double AvgAge { get; set; }
    }
}
