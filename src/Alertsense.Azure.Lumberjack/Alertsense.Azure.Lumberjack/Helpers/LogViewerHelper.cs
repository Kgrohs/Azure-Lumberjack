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
            
            var filteredLogs = filterModel.LogLevel != null ? unfilteredLogs.Where(x => ( x.Level != null && x.Level.ToLowerInvariant() == filterModel.LogLevel.ToLowerInvariant())) : unfilteredLogs; 
            filteredLogs = filterModel.Thread != null ? filteredLogs.Where(x => ( x.Thread != null && x.Thread.ToLowerInvariant() == filterModel.Thread.ToLowerInvariant())) : filteredLogs; 
            filteredLogs = filterModel.LoggerType != null ? 
                filteredLogs.Where(x => (x.Logger != null && x.Logger.ToLowerInvariant() == filterModel.LoggerType.ToLowerInvariant())) : 
                filteredLogs;
            if(filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                filteredLogs = filteredLogs.Where(x => x.Date >= filterModel.StartDate && x.Date <= filterModel.EndDate); // EndDate.AddSeconds(1) ??
            else { 
                filteredLogs = filterModel.StartDate.HasValue ? filteredLogs.Where(x => x.Date >= filterModel.StartDate) : filteredLogs;
                filteredLogs = filterModel.EndDate.HasValue ? filteredLogs.Where(x => x.Date <= filterModel.EndDate) : filteredLogs; // EndDate.AddSeconds(1) ??
            }
            return filteredLogs.ToList();
        }
    }
}




