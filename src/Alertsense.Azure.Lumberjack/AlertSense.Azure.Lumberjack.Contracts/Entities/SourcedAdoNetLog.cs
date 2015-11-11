namespace AlertSense.Azure.Lumberjack.Contracts.Entities
{
    public class SourcedAdoNetLog : AdoNetLog
    {
        public string Source { get; set; }
    }
}