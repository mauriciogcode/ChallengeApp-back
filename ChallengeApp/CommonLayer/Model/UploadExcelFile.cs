namespace ChallengeApp.CommonLayer.Model
{
    public class UploadExcelFileRequest
    {
        public IFormFile File { get; set; }
    }

    public class UploadExcelFileResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ExcelBulkUploadParameter
    {
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Team { get; set; }
        public string State { get; set; }
        public string Education { get; set; }
    }
}
