namespace ChallengeApp.CommonLayer.Model
{
    public class DeleteRecordRequest
    {
        public int UserId { get; set; }
    }

    public class DeleteRecordResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
