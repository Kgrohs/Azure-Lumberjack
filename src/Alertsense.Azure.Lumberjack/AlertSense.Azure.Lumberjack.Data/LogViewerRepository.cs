using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSense.Azure.Lumberjack.Contracts.Entities;
using AlertSense.Azure.Lumberjack.Contracts.Repositories;
using ServiceStack.Logging;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Dapper;

namespace AlertSense.Azure.Lumberjack.Data
{
    public class LogViewerRepository : BaseRepository, ILogViewerRepository
    {
        readonly ILog _log = LogManager.GetLogger(typeof(LogViewerRepository));

        public IEnumerable<AdoNetLog> GetAllLogs()
        {
            try
            {
                var qryResults = DbConnection.Query<AdoNetLog>("SELECT TOP 50 * FROM Log ORDER BY Date DESC").ToList();
                return qryResults;
            }
            catch (SqlException e)
            {
                return new List<AdoNetLog>();
            }
        }

        public IEnumerable<SourcedAdoNetLog> GetAllLogs(string tableName, string conn)
        {
            try
            {
                var qryResults = DbConnection.Query<SourcedAdoNetLog>("SELECT TOP 50 [Id],[Date],[Thread],[Level],[Logger],[Message],[Exception] FROM " + tableName +" ORDER BY Date DESC").ToList();
                foreach (var q in qryResults)
                {
                    q.Source = conn;
                }
                return qryResults;
            }
            catch (SqlException e)
            {
                _log.Error(e.Message, e);
                return new List<SourcedAdoNetLog>();
            }
        }

        public IEnumerable<string> GetDistinctLoggersList(string tableName)
        {
            try
            {
                var qryResults = DbConnection.Query<string>("SELECT DISTINCT Logger FROM " + tableName).ToList();
                return qryResults;
            }
            catch (SqlException e)
            {
                return new List<string>();
            }
        }

        public IEnumerable<SourcedAdoNetLog> FilteredLogs(
            string tableName,
            string conn,
            string logLevel,
            DateTime? startDate,
            DateTime? endDate,
            string loggerType,
            string thread
            )
        {
            var sql = "SELECT * FROM Log";
            sql += " WHERE Id > -1";
            if (!String.IsNullOrEmpty(logLevel)) sql += " AND Level = @loglevel";
            if (startDate.HasValue)
            {
                sql += " AND Date >= @startdate";
            }

            if (endDate.HasValue)
            {
                sql += " AND Date < @enddate";
            }
            if (!String.IsNullOrEmpty(thread)) sql += " AND Thread = @thread";
            if (!String.IsNullOrEmpty(loggerType)) sql += " AND Logger = @loggertype";

            try
            {
                var logs = DbConnection.Query<SourcedAdoNetLog>(
                    sql,
                    new
                    {
                        
                        loglevel = logLevel,
                        startdate = startDate.HasValue ? startDate.Value.ToString(CultureInfo.InvariantCulture) : null,
                        enddate =
                            endDate.HasValue ? endDate.Value.AddSeconds(1).ToString(CultureInfo.InvariantCulture) : null,
                        thread,
                        loggertype = loggerType
                    }
                    );
                var sourcedAdoNetLogs = logs as IList<SourcedAdoNetLog> ?? logs.ToList();
                foreach (var q in sourcedAdoNetLogs)
                {
                    q.Source = conn;
                }
                return sourcedAdoNetLogs;
            }
            catch (SqlException e)
            {
                return new List<SourcedAdoNetLog>();
            }
        }


        
    }
}
