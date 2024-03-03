namespace ChallengeApp.CommonLayer.Model
{
    public class UploadCsvFileRequest
    {
        public IFormFile File { get; set; }
    }

    public class UploadCsvFileResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
