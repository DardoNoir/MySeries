using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AuditLogging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace MySeries.Monitoring
{
    public class MonitoringAppService : MySeriesAppService, IApiMonitoringAppService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public MonitoringAppService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        // Traer las estadísticas de la API
        public async Task<MonitoringDto> GetApiStatsAsync()
        {
            var logs = await _auditLogRepository.GetListAsync(
                startTime: DateTime.Now.AddDays(-7)   // de la última semana
            );

            // 5 Endpoints más llamados
            var topEndpoints = logs
                .Where(l => l.Url != null)
                .GroupBy(l => l.Url)
                .Select(g => new EndpointStatDto
                {
                    Method = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            // Llamadas a la API de Omdb
            var omdbCalls = logs.Count(l =>
                l.Url != null && l.Url.Contains("search", StringComparison.OrdinalIgnoreCase));

            // Cantidad de errores
            var errorCount = logs.Count(x => x.Exceptions != null);
            if (errorCount > 0)
            {
                Logger.LogWarning($"DIAGNÓSTICO: Se han detectado {errorCount} errores en la API en la última semana.");
            }

            // Devolver el Dto de Monitoreo
            return new MonitoringDto
            {
                TotalCalls = logs.Count,
                AverageResponseTime = logs.Any() ? logs.Average(x => x.ExecutionDuration) : 0,
                ErrorCount = errorCount,
                TopEndpoints = topEndpoints,
                OmdbApiConsumptions = omdbCalls
            };
        }
    }
}