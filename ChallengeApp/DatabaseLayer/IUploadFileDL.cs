using ChallengeApp.CommonLayer.Model;

namespace ChallengeApp.DatabaseLayer
{
    public interface IUploadFileDL
    {
        public Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request, string Path);

        public Task<ReadRecordResponse> ReadRecord(ReadRecordRequest request);
        public Task<ReadAvgAgeRecordResponse> ReadAvgAge(ReadAvgAgeRecordRequest request);
        public Task<SearchByStateAndEducationResponse> SearchByStateAndEducation(SearchByStateAndEducationRequest request);
        public Task<DeleteRecordResponse> DeleteRecord(DeleteRecordRequest request);

        public Task<UploadCsvFileResponse> UploadCsvFile(UploadCsvFileRequest request, string Path);
        Task<SearchCommonNamesResponse> SearchCommonNames(SearchCommonNamesRequest request);
        Task<ReadTeamStatisticsRecordResponse> ReadTeamStatisticsRecord();
    }
}
