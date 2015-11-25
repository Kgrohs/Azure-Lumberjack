using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSense.Azure.Lumberjack.Contracts.Entities;

namespace AlertSense.Azure.Lumberjack.Contracts.Managers
{
    public interface ILogViewerManager
    {
        IEnumerable<AdoNetLog> GetAllLogs();
        IEnumerable GetLogLevelList();
        IEnumerable<string> GetDistinctLoggersList(string tableName);
    }
}
