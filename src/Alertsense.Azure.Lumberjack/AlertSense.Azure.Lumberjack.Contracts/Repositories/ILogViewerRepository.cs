﻿using System;
using System.Collections.Generic;
using AlertSense.Azure.Lumberjack.Contracts.Entities;
using ServiceStack.Data;

namespace AlertSense.Azure.Lumberjack.Contracts.Repositories
{
    public interface ILogViewerRepository
    {
        IDbConnectionFactory DbFactory { get; set; }

        IEnumerable<AdoNetLog> GetAllLogs();
        IEnumerable<SourcedAdoNetLog> GetAllLogs(string tableName, string connMap);
        IEnumerable<string> GetDistinctLoggersList(string tableName);
        IEnumerable<SourcedAdoNetLog> FilteredLogs(string tableName,
            string conn,
            string logLevel,
            DateTime? startDate,
            DateTime? endDate,
            string loggerType,
            string thread);
    }
}
