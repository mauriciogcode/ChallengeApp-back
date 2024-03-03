namespace ChallengeApp.CommonLayer.Model
{
    public class SearchByStateAndEducationRequest
    {
        public string State { get; set; }
        public string Education { get; set; }
    }

    public class SearchByStateAndEducationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<SearchResult> Results { get; set; }
    }

    public class SearchResult
    {
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Team { get; set; }
    }
}
