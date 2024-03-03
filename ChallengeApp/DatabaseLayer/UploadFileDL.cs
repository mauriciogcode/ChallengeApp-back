using ChallengeApp.CommonLayer.Model;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;
using System.Globalization;


namespace ChallengeApp.DatabaseLayer
{
    public class UploadFileDL : IUploadFileDL
    {
        public readonly IConfiguration _configuration;
        public readonly MySqlConnection _mySqlConnection;


        public UploadFileDL(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new MySqlConnection(_configuration["ConnectionStrings:MySqlDBConnectionString"]);
        }

        #region DeleteRecord
        public async Task<DeleteRecordResponse> DeleteRecord(DeleteRecordRequest request)
        {
            DeleteRecordResponse response = new DeleteRecordResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                string SqlQuery = @"DELETE FROM challenge.bulkuploadtable WHERE UserId = @UserId";
                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 100;
                    sqlCommand.Parameters.AddWithValue("@UserId", request.UserId);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Query Not Executed";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                _mySqlConnection.CloseAsync();
                _mySqlConnection.DisposeAsync();
            }
            return response;

        }
        #endregion

        #region ReadRecord
        public async Task<ReadRecordResponse> ReadRecord(ReadRecordRequest request)
        {
            ReadRecordResponse response = new ReadRecordResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            int Count = 0;

            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT DISTINCT UserId, 
		                                           UserName, 
			                                       Age, 
			                                       Team, 
			                                       State, 
			                                       Education, 
			                                       (SELECT COUNT(*) FROM challenge.bulkuploadtable) AS TotalRecord
		                                    FROM challenge.bulkuploadtable
		                                    LIMIT @Offset, @RecordPerPage;
                                             ";


                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    int Offset = (request.PageNumber - 1) * request.RecordPerPage;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@Offset", Offset);
                    sqlCommand.Parameters.AddWithValue("@RecordPerPage", request.RecordPerPage);
                    using (DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            response.readRecord = new List<ReadRecord>();
                            while (await sqlDataReader.ReadAsync())
                            {


                                ReadRecord getdata = new ReadRecord();
                                getdata.UserId = sqlDataReader["UserId"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["UserID"]) : -1;
                                getdata.UserName = sqlDataReader["UserName"] != DBNull.Value ? Convert.ToString(sqlDataReader["UserName"]) : "-1";
                                getdata.Age = sqlDataReader["Age"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["Age"]) : -1;
                                getdata.Team = sqlDataReader["Team"] != DBNull.Value ? Convert.ToString(sqlDataReader["Team"]) : "-1";
                                getdata.State = sqlDataReader["State"] != DBNull.Value ? Convert.ToString(sqlDataReader["State"]) : "-1";
                                getdata.Education = sqlDataReader["Education"] != DBNull.Value ? Convert.ToString(sqlDataReader["Education"]) : "-1";
                                if (Count == 0)
                                {
                                    Count++;
                                    response.TotalRecords = sqlDataReader["TotalRecord"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["TotalRecord"]) : 0;
                                    response.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(response.TotalRecords / request.RecordPerPage)));
                                    response.CurrentPage = request.PageNumber;
                                }
                                response.readRecord.Add(getdata);
                            }
                        }
                        else
                        {
                            //response.IsSuccess = false;
                            response.Message = "Record Not Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }


        #endregion

        #region ReadAvgAgeRecord
        public async Task<ReadAvgAgeRecordResponse> ReadAvgAge(ReadAvgAgeRecordRequest request)
        {
            ReadAvgAgeRecordResponse response = new ReadAvgAgeRecordResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT round(avg(Age)) AS AvgAge
                            FROM challenge.bulkuploadtable
                            WHERE Team = @Team";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 100;
                    sqlCommand.Parameters.AddWithValue("@Team", request.Team);

                    using (DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.HasRows && await sqlDataReader.ReadAsync())
                        {
                            response.AvgAge = sqlDataReader["AvgAge"] != DBNull.Value ? Convert.ToDouble(sqlDataReader["AvgAge"]) : -1;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Record Not Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }


        #endregion

        #region TeamStadistics
        public async Task<ReadTeamStatisticsRecordResponse> ReadTeamStatisticsRecord()
        {
            ReadTeamStatisticsRecordResponse response = new ReadTeamStatisticsRecordResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT Team, AVG(Age) AS AverageAge, MIN(Age) AS MinAge, MAX(Age) AS MaxAge, COUNT(*) AS NumberOfMembers
                                    FROM challenge.bulkuploadtable
                                    GROUP BY Team
                                    ORDER BY COUNT(*) DESC, Team ASC";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 100;

                    using (DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            response.TeamStatisticsList = new List<TeamStatistics>();

                            while (await sqlDataReader.ReadAsync())
                            {
                                TeamStatistics teamStatistics = new TeamStatistics();
                                teamStatistics.TeamName = sqlDataReader["Team"] != DBNull.Value ? Convert.ToString(sqlDataReader["Team"]) : "-1";
                                teamStatistics.AverageAge = sqlDataReader["AverageAge"] != DBNull.Value ? Convert.ToDouble(sqlDataReader["AverageAge"]) : 0;
                                teamStatistics.MinAge = sqlDataReader["MinAge"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["MinAge"]) : 0;
                                teamStatistics.MaxAge = sqlDataReader["MaxAge"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["MaxAge"]) : 0;
                                teamStatistics.NumberOfMembers = sqlDataReader["NumberOfMembers"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["NumberOfMembers"]) : 0;
                                response.TeamStatisticsList.Add(teamStatistics);
                            }
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No records found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }
        #endregion

        #region SearchByStateAndEducation
        public async Task<SearchByStateAndEducationResponse> SearchByStateAndEducation(SearchByStateAndEducationRequest request)
        {
            SearchByStateAndEducationResponse response = new SearchByStateAndEducationResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT UserName, Age, Team
                                    FROM challenge.bulkuploadtable
                                    WHERE State = @State AND Education = @Education
                                    ORDER BY UserName
                                    LIMIT 100;";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 100;
                    sqlCommand.Parameters.AddWithValue("@State", request.State);
                    sqlCommand.Parameters.AddWithValue("@Education", request.Education);

                    using (DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            response.Results = new List<SearchResult>();

                            while (await sqlDataReader.ReadAsync())
                            {
                                SearchResult result = new SearchResult();
                                result.UserName = sqlDataReader["UserName"] != DBNull.Value ? Convert.ToString(sqlDataReader["UserName"]) : "-1";
                                result.Age = sqlDataReader["Age"] != DBNull.Value ? Convert.ToInt32(sqlDataReader["Age"]) : -1;
                                result.Team = sqlDataReader["Team"] != DBNull.Value ? Convert.ToString(sqlDataReader["Team"]) : "-1";
                                response.Results.Add(result);
                            }
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No records found matching the given criteria.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }
        #endregion

        #region SearchCommonNames
        public async Task<SearchCommonNamesResponse> SearchCommonNames(SearchCommonNamesRequest request)
        {
            SearchCommonNamesResponse response = new SearchCommonNamesResponse();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT UserName
                            FROM challenge.bulkuploadtable
                            WHERE Team = @Team
                            GROUP BY UserName
                            ORDER BY COUNT(UserName) DESC
                            LIMIT 5";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandTimeout = 100;
                    sqlCommand.Parameters.AddWithValue("@Team", request.Team);

                    using (DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            response.MostCommonNames = new List<NameCount>();

                            while (await sqlDataReader.ReadAsync())
                            {
                                NameCount nameCount = new NameCount();
                                nameCount.UserName = sqlDataReader["UserName"] != DBNull.Value ? Convert.ToString(sqlDataReader["UserName"]) : "-1";
                                response.MostCommonNames.Add(nameCount);
                            }
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "No records found for the given team.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }
        #endregion



        #region UploadCVSFile
        public async Task<UploadCsvFileResponse> UploadCsvFile(UploadCsvFileRequest request, string Path)
        {
            UploadCsvFileResponse response = new UploadCsvFileResponse();
            List<ExcelBulkUploadParameter> Parameters = new List<ExcelBulkUploadParameter>();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                using (var stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    var reader = new StreamReader(stream);
                    var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
                    csvConfig.Delimiter = ";";

                    var csv = new CsvReader(reader, csvConfig);
                    while (csv.Read())
                    {
                        var row = csv.Context.Parser.Record;

                        var parameter = new ExcelBulkUploadParameter();
                        parameter.UserName = row[0];
                        parameter.Age = Convert.ToInt32(row[1]);
                        parameter.Team = row[2];
                        parameter.State = row[3];
                        parameter.Education = row[4];

                        Parameters.Add(parameter);
                    }
                }

                if (Parameters.Count > 0)
                {
                    await InsertBatchIntoDatabase(Parameters);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

        private async Task InsertBatchIntoDatabase(List<ExcelBulkUploadParameter> Parameters)
        {

            using (var transaction = _mySqlConnection.BeginTransaction())
            {
                try
                {
                    string sqlQuery = @"INSERT INTO challenge.bulkuploadtable (UserName, Age, Team, State, Education) 
                               VALUES (@UserName, @Age, @Team, @State, @Education)";

                    int batchSize = 1000; // Sizeable


                    using (var command = new MySqlCommand(sqlQuery, _mySqlConnection, transaction))
                    {
                        command.Parameters.Add("@UserName", MySqlDbType.VarChar);
                        command.Parameters.Add("@Age", MySqlDbType.Int32);
                        command.Parameters.Add("@Team", MySqlDbType.VarChar);
                        command.Parameters.Add("@State", MySqlDbType.VarChar);
                        command.Parameters.Add("@Education", MySqlDbType.VarChar);

                        for (int i = 0; i < Parameters.Count; i += batchSize)
                        {
                            var batchParameters = Parameters.GetRange(i, Math.Min(batchSize, Parameters.Count - i));

                            command.Parameters["@UserName"].Value = DBNull.Value;
                            command.Parameters["@Age"].Value = DBNull.Value;
                            command.Parameters["@Team"].Value = DBNull.Value;
                            command.Parameters["@State"].Value = DBNull.Value;
                            command.Parameters["@Education"].Value = DBNull.Value;

                            foreach (var parameter in batchParameters)
                            {
                                command.Parameters["@UserName"].Value = parameter.UserName;
                                command.Parameters["@Age"].Value = parameter.Age;
                                command.Parameters["@Team"].Value = parameter.Team;
                                command.Parameters["@State"].Value = parameter.State;
                                command.Parameters["@Education"].Value = parameter.Education;

                                await command.ExecuteNonQueryAsync();
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        #endregion


        #region UploadExcelFile
        public async Task<UploadExcelFileResponse> UploadExcelFile(UploadExcelFileRequest request, string Path)
        {
            UploadExcelFileResponse response = new UploadExcelFileResponse();
            List<ExcelBulkUploadParameter> Parameters = new List<ExcelBulkUploadParameter>();
            response.IsSuccess = true;
            response.Message = "Successful";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                if (request.File.FileName.ToLower().Contains(".xls"))
                {
                    FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                    DataSet dataset = reader.AsDataSet(
                        new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = false,
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                    {
                        ExcelBulkUploadParameter rows = new ExcelBulkUploadParameter();
                        rows.UserName = dataset.Tables[0].Rows[i].ItemArray[0] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[0]) : "- 1";
                        rows.Age = dataset.Tables[0].Rows[i].ItemArray[1] != null ? Convert.ToInt32(dataset.Tables[0].Rows[i].ItemArray[1]) : -1;
                        rows.Team = dataset.Tables[0].Rows[i].ItemArray[2] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[2]) : "- 1";
                        rows.State = dataset.Tables[0].Rows[i].ItemArray[3] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[3]) : "- 1";
                        rows.Education = dataset.Tables[0].Rows[i].ItemArray[4] != null ? Convert.ToString(dataset.Tables[0].Rows[i].ItemArray[4]) : "- 1";
                        Parameters.Add(rows);

                    }
                    stream.Close();

                    if (Parameters.Count > 0)
                    {
                        string SqlQuery = @"INSERT INTO challenge.bulkuploadtable (UserName, Age, Team, State, Education) 
                                                 VALUES (@UserName, @Age, @Team, @State, @Education)";
                        foreach (ExcelBulkUploadParameter rows in Parameters)
                        {
                            using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                            {
                                sqlCommand.CommandType = CommandType.Text;
                                sqlCommand.CommandTimeout = 100;
                                sqlCommand.Parameters.AddWithValue("@UserName", rows.UserName);
                                sqlCommand.Parameters.AddWithValue("@Age", rows.Age);
                                sqlCommand.Parameters.AddWithValue("@Team", rows.Team);
                                sqlCommand.Parameters.AddWithValue("@State", rows.State);
                                sqlCommand.Parameters.AddWithValue("@Education", rows.Education);
                                int Status = await sqlCommand.ExecuteNonQueryAsync();
                                if (Status == 0)
                                {
                                    response.IsSuccess = false;
                                    response.Message = "Query not executed";
                                    return response;
                                }
                            }
                        }
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Incorrect File";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }
        #endregion


    }

}







