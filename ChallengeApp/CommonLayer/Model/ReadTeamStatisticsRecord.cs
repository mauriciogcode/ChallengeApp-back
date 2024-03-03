namespace ChallengeApp.CommonLayer.Model
{

    public class ReadTeamStatisticsRecordResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<TeamStatistics> TeamStatisticsList { get; set; }
    }

    public class TeamStatistics
    {
        public string TeamName { get; set; }
        public double AverageAge { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int NumberOfMembers { get; set; }
    }
}
