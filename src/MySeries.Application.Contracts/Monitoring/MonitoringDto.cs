using System;
namespace MySeries.MonitoringService
{
    public class MonitoringDto
    {
        public string? Endpoint { get; set; }
        public long AverageResponseTime { get; set; }
        public int RequestCount { get; set; }
        public int ErrorCount { get; set; }
    }
}


