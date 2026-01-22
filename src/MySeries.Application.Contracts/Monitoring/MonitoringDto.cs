using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Monitoring
{
    public class MonitoringDto
    {
        public int TotalCalls { get; set; }
        public double AverageResponseTime { get; set; }
        public int ErrorCount { get; set; }
        public List<EndpointStatDto>? TopEndpoints { get; set; }
        public int OmdbApiConsumptions { get; set; }
    }

    public class EndpointStatDto
    {
        public string? Method { get; set; }
        public int Count { get; set; }
    }
}
