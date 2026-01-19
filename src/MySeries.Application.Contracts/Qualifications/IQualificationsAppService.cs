using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.Qualifications
{
    public interface IQualificationsAppService : IApplicationService
    {
        Task QualificationsSeriesAsync(int userId, int serieId, int Score, string? Review = null);

        Task ModifyQualificationAsync(int userId, int serieId, int Score, string? Review = null);

    }
}