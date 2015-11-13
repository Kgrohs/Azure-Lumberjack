using System;
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
                _log.Error(e.Message, e);
                return new List<AdoNetLog>();
            }
        }

        public IEnumerable<SourcedAdoNetLog> GetAllLogs(string tableName, string connMap)
        {
            try
            {
                var qryResults = DbConnection.Query<SourcedAdoNetLog>("SELECT TOP 50 [Id],[Date],[Thread],[Level],[Logger],[Message],[Exception] FROM " + tableName +" ORDER BY Date DESC").ToList();
                //TODO: consider changing this foreach to happen in query
                foreach (var q in qryResults)
                {
                    q.Source = connMap;
                }
                return qryResults;
            }
            catch (SqlException e)
            {
                _log.Error(e.Message, e);
                return new List<SourcedAdoNetLog>();
            }
        }
    }
}
