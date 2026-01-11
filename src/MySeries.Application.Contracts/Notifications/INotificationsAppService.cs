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
        Task<List<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task SendNotificationAsync(int userId, string message);
        Task NotifyByEmailAsync(int userId, string message);
        Task MarkReadenAsync(int notificationId);
    }
}
