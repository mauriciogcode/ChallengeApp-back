namespace ChallengeApp.CommonLayer.Model
{
    public class SearchCommonNamesRequest
    {
        public string Team { get; set; }
    }

    public class SearchCommonNamesResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<NameCount> MostCommonNames { get; set; }
    }

    public class NameCount
    {
        public string UserName { get; set; }
    }
}
