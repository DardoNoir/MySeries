using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using MySeries.MonitoringService;

namespace Myseries.Monitoring
{
    public interface IMonitoringAppService 
    {
       Task <List<MonitoringDto>> GetMonitoringDataAsync(); 
       
    }
}