using System;
using System.Collections.Generic;
using AlertSense.Azure.Lumberjack.Contracts.Entities;
using ServiceStack.Data;

namespace AlertSense.Azure.Lumberjack.Contracts.Repositories
{
    public interface ILogViewerRepository
    {
        IDbConnectionFactory DbFactory { get; set; }

        //List<string> GetDistinctLoggersList();

        //List<AdoNetLog> GetLogsByIds(List<string> ids);

        //int GetCount(
        //    string logLevel,
        //    DateTime? startDate,
        //    DateTime? endDate,
        //    string loggerType,
        //    int? regionId,
        //    int? userId,
        //    string thread
        //);

        //List<AdoNetLog> PagedFilteredLogs(
        //    string logLevel,
        //    DateTime? startDate,
        //    DateTime? endDate,
        //    string loggerType,
        //    int? regionId,
        //    int? userId,
        //    string thread,
        //    int pageNumber,
        //    int pageSize
        //);

        //bool ColumnExists(string columnName);
        IEnumerable<AdoNetLog> GetAllLogs();
        IEnumerable<SourcedAdoNetLog> GetAllLogs(string tableName, string connMap);
    }
}
