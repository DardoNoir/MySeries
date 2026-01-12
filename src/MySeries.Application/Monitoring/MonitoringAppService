using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Myseries.Monitoring
{
    public class MonitoringAppService : MySeriesAppService, IMonitoringAppService
    {
        private static readonly ConcurrentDictionary<string, List<long>> _RequestTiming = new();
        private static readonly ConcurrentDictionary<string, int> _ErrorCounts = new();

        public static void RecordRequestTiming(string endpoint, long milliseconds, bool isError)
        {
            if (!_RequestTiming.ContainsKey(endpoint))
            {
                _RequestTiming[endpoint] = new List<long>();
                _ErrorCounts[endpoint] = 0;
            }

            _RequestTiming[endpoint].Add(milliseconds);
            if (isError)
            {
                _ErrorCounts[endpoint]++;
            }
        }
        
        public async Task<List<MonitoringDto>> GetMonitoringDataAsync()
        {
            
            var result = _RequestTiming.Select(kvp => new MonitoringRecordDto
            {
                Endpoint = kvp.Key,
                AverageResponseTime = kvp.Value.Any() ? (long)Math.Round(kvp.Value.Average()) : 0, // Redondea el promedio
                RequestCount = kvp.Value.Count,
                ErrorCount = _ErrorCounts[kvp.Key]
            }).ToList();

            return Task.FromResult(result);
        }
    }
}