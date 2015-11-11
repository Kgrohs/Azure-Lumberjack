using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlertSense.Azure.Lumberjack.Contracts.Entities;

namespace Alertsense.Azure.Lumberjack.Models
{
    public class LogViewerViewModel
    {
        public IEnumerable<SourcedAdoNetLog> Logs { get; set; }
        public IEnumerable<string> ConnectionList { get; set; }

    }
}