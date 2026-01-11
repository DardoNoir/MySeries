using MySeries.Notifications;
using MySeries.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;

namespace MySeries.NotificationsAppService
{
    public class NotificationsAppService : INotificationsAppService
    {
        private readonly IRepository<Notification, int> _notificationRepository;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Usuario, int> _userRepository;

        public NotificationsAppService(
            IRepository<Notification, int> notificationRepository,
            IEmailSender emailSender,
            IRepository<Usuario, int> userRepository)
        {
            _notificationRepository = notificationRepository;
            _emailSender = emailSender;
            _userRepository = userRepository;
        }


        public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            if (userId <= 0) 
                {
                    throw new Exception("Usuario no autenticado");
            }
            // Obtener las notificaciones no leidas del usuario desde el repositorio
            var notifications = await _notificationRepository.GetListAsync(n => n.UserId == userId && !n.IsRead);
            var notificationDtos = notifications.Select(n => new NotificationDto(userId, n.Message)
            {
                CreatedAt = n.CreatedAt
            }).ToList();

            return notificationDtos;
        }


        public async Task SendNotificationAsync(int userId, string message)
        {
            var user = await _userRepository.GetAsync(userId);

            if (!user.NotificationsByApp)
                throw new Exception("El usuario no permite notificaciones por app");

            var notification = new Notification(userId, message);

            await _notificationRepository.InsertAsync(notification);
        }

        public async Task NotifyByEmailAsync(int userId, string message)
        {
            var user = await _userRepository.GetAsync(userId);

            if (!user.NotificationsByEmail)
                throw new Exception("El usuario no permite notificaciones por email");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("El usuario no tiene un email registrado");

            await _emailSender.SendAsync(
                user.Email,
                "Nueva Notificación",
                message
            );
        }

        public async Task MarkReadenAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetAsync(notificationId);

            notification.MarkAsRead();

            await _notificationRepository.UpdateAsync(notification);
        }

    }
}
