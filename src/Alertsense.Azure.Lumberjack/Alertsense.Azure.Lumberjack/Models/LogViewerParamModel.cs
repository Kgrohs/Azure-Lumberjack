using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alertsense.Azure.Lumberjack.Models
{
    public class LogViewerParamModel
    {
        public string LogLevel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LoggerType { get; set; }
        public string Thread { get; set; }
        public string Lumberjack { get; set; }
        public string JustBlogging { get; set; }
    }
}