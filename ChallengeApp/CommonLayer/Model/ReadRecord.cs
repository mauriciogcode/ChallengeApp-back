namespace ChallengeApp.CommonLayer.Model
{
    public class ReadRecordRequest
    {
        public int PageNumber { get; set; }
        public int RecordPerPage { get; set; }
    }

    public class ReadRecordResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int CurrentPage { get; set; }
        public double TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public List<ReadRecord> readRecord { get; set; }
    }

    public class ReadRecord
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Team { get; set; }
        public string State { get; set; }
        public string Education { get; set; }
    }
}
