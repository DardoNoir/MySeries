using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MySeries.Notifications
{
    public interface INotificationsAppService: IApplicationService
    {
        // task SendNotificationAsync(int userId, string message);
        // task NotifyByEmailAsync(int userId, string message);
        // task MarkReadenAsync(int notificationId);
    }
}
