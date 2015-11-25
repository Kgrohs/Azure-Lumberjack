using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSense.Azure.Lumberjack.Contracts.Entities;
using AlertSense.Azure.Lumberjack.Contracts.Managers;
using AlertSense.Azure.Lumberjack.Contracts.Repositories;
using AlertSense.Azure.Lumberjack.Data;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;

namespace AlertSense.Azure.Lumberjack.Services
{
    public static class LogViewerManagerFactory
    {
        public static LogViewerManager CreateManager(ILogViewerRepository repository = null, IDbConnectionFactory connectionFactory = null, string connectionStringMapping = null)
        {
            if (repository == null)
                repository = new LogViewerRepository();

            if (connectionStringMapping == null)
                connectionStringMapping = "LoggingConnection1";

            if (connectionFactory == null && repository.DbFactory == null)
            {
                if (ConfigurationManager.ConnectionStrings[connectionStringMapping] == null)
                {
                    connectionFactory =
                        new OrmLiteConnectionFactory(
                            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
                            SqlServerOrmLiteDialectProvider.Instance);
                    repository.DbFactory = connectionFactory;
                }
                else
                {
                    connectionFactory =
                        new OrmLiteConnectionFactory(
                            ConfigurationManager.ConnectionStrings[connectionStringMapping].ConnectionString,
                            SqlServerOrmLiteDialectProvider.Instance);

                    repository.DbFactory = connectionFactory;
                }
            }

            return new LogViewerManager
            {
                LogViewerRepo = repository
            };
        }
    }

    public class LogViewerManager : ILogViewerManager
    {
        public ILogViewerRepository LogViewerRepo { get; set; }

        public IEnumerable<AdoNetLog> GetAllLogs()
        {
            var logs = LogViewerRepo.GetAllLogs();
            return logs;
        }

        public IEnumerable<SourcedAdoNetLog> GetAllLogs(string tableName, string connMap)
        {
            var logs = LogViewerRepo.GetAllLogs(tableName, connMap);
            return logs;
        }

        public  IEnumerable GetLogLevelList()
        {
            var list = Enum.GetNames(typeof(Level)).ToList();
            return list;
        }
        private enum Level
        {
            Debug = 5,
            Info = 4,
            Warn = 3,
            Error = 2,
            Fatal = 1,
        }

        public IEnumerable<string> GetDistinctLoggersList(string tableName)
        {
            var logs = LogViewerRepo.GetDistinctLoggersList(tableName);
            return logs;
        }
    }
}
