using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alertsense.Azure.Lumberjack.Models;
using AlertSense.Azure.Lumberjack.Contracts.Entities;

namespace Alertsense.Azure.Lumberjack.Helpers
{
    public static class LogViewerHelper
    {
        public static List<SourcedAdoNetLog> FilterLogs(this List<SourcedAdoNetLog> unfilteredLogs , LogViewerParamModel filterModel )
        {
            var filteredLogs = filterModel.StartDate.HasValue ? unfilteredLogs.Where(x => x.Date >= filterModel.StartDate) : unfilteredLogs;
            filteredLogs = filterModel.EndDate.HasValue ? unfilteredLogs.Where(x => x.Date <= filterModel.EndDate) : filteredLogs; // EndDate.AddSeconds(1) ??
            filteredLogs = filterModel.LogLevel != null ? unfilteredLogs.Where(x => x.Level.ToLowerInvariant() == filterModel.LogLevel.ToLowerInvariant()) : filteredLogs; 
            filteredLogs = filterModel.Thread != null ? unfilteredLogs.Where(x => x.Thread.ToLowerInvariant() == filterModel.Thread.ToLowerInvariant()) : filteredLogs; 
            filteredLogs = filterModel.LoggerType != null ? unfilteredLogs.Where(x => x.Logger.ToLowerInvariant() == filterModel.LoggerType.ToLowerInvariant()) : filteredLogs; 

            return filteredLogs.ToList();
        }
    }
}




//TODO *********************** Done? test it *************************************