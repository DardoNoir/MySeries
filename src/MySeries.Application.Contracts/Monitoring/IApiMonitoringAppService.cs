using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.Monitoring
{
    public interface IApiMonitoringAppService :IApplicationService
    {
        Task<MonitoringDto> GetApiStatsAsync();
    }
}
