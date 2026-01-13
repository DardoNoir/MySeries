using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.Qualifications
{
    public interface IQualificationsAppService: IApplicationService
    {
        Task QualificationsSeriesAsync(Guid serieId, int Score, string? Review = null);

        Task ModifyQualificationAsync(Guid serieId, int Score, string? Review = null);

    }
}
