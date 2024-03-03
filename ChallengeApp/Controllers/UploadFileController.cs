using ChallengeApp.CommonLayer.Model;
using ChallengeApp.DatabaseLayer;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeApp.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UploadFileController : Controller
    {
        public readonly IUploadFileDL _uploadFileDL;

        public UploadFileController(IUploadFileDL uploadFileDL)
        {
            _uploadFileDL = uploadFileDL;
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UploadExcelFile([FromForm] UploadExcelFileRequest request)
        {
            UploadExcelFileResponse response = new UploadExcelFileResponse();
            string Path = "UploadFileFolder/" + request.File.FileName;
            try
            {
                using (FileStream stream = new FileStream(Path, FileMode.CreateNew))
                {
                    await request.File.CopyToAsync(stream);
                }
                response = await _uploadFileDL.UploadExcelFile(request, Path);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> UploadCsvFile([FromForm] UploadCsvFileRequest request)
        {
            UploadCsvFileResponse response = new UploadCsvFileResponse();
            string Path = "UploadFileFolder/" + request.File.FileName;
            try
            {
                using (FileStream stream = new FileStream(Path, FileMode.CreateNew))
                {
                    await request.File.CopyToAsync(stream);
                }
                response = await _uploadFileDL.UploadCsvFile(request, Path);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> ReadRecord(ReadRecordRequest request)
        {
            ReadRecordResponse response = new ReadRecordResponse();
            try
            {
                response = await _uploadFileDL.ReadRecord(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ReadAvgAge(ReadAvgAgeRecordRequest request)
        {
            ReadAvgAgeRecordResponse response = new ReadAvgAgeRecordResponse();
            try
            {
                response = await _uploadFileDL.ReadAvgAge(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> ReadTeamStatisticsRecord()
        {
            ReadTeamStatisticsRecordResponse response = new ReadTeamStatisticsRecordResponse();
            try
            {
                response = await _uploadFileDL.ReadTeamStatisticsRecord();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByStateAndEducation(SearchByStateAndEducationRequest request)
        {
            SearchByStateAndEducationResponse response = new SearchByStateAndEducationResponse();
            try
            {
                response = await _uploadFileDL.SearchByStateAndEducation(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SearchCommonNames(SearchCommonNamesRequest request)
        {
            SearchCommonNamesResponse response = new SearchCommonNamesResponse();
            try
            {
                response = await _uploadFileDL.SearchCommonNames(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRecord(DeleteRecordRequest request)
        {
            DeleteRecordResponse response = new DeleteRecordResponse();
            try
            {
                response = await _uploadFileDL.DeleteRecord(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }
    }
}
