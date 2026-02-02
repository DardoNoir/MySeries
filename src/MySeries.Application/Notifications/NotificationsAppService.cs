using Microsoft.Extensions.Configuration.UserSecrets;
using MySeries.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;

namespace MySeries.Notifications
{
    public class NotificationsAppService :  ApplicationService ,INotificationsAppService
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

        // Traer notificaciones NO leídas
        public async Task<List<NotificationDto>> GetUnreadAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new Exception("Usuario no autenticado");
            }
            var notifications = await _notificationRepository.GetListAsync(n => n.UserId == userId && !n.IsRead);
            var notificationDtos = notifications.Select(n => new NotificationDto(userId, n.Message)
            {
                Id=n.Id,
                CreatedAt = n.CreatedAt
            })
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
            return notificationDtos;
        }

        // Traer Todas las notificaciones notificaciones
        public async Task<List<NotificationDto>> GetAllAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new Exception("Usuario no autenticado");
            }
            var Notifications = await _notificationRepository.GetListAsync(n => n.UserId == userId);
            var notificationDtos = Notifications.Select(n => new NotificationDto(userId, n.Message)
            {
                Id=n.Id,
                CreatedAt = n.CreatedAt
            })
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
            return notificationDtos;
        }

        // Cantidad de Notificaciones NO leídas
        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _notificationRepository.CountAsync(
                n => n.UserId == userId && !n.IsRead
            );
        }

        /*
        Se deshabilitó el servicio, ya que se creó un Controlador
        que luego será usado en el Frontend
        */
        [RemoteService(IsEnabled = false)] 
        // Envío de Notificaciones por aplicación
        public async Task SendNotificationAsync(int userId, string message)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            if (!user.NotificationsByApp)
                throw new Exception("El usuario no permite notificaciones por app");

            var notification = new Notification(userId, message);

            await _notificationRepository.InsertAsync(notification);
        }

        /*
        Se deshabilitó el servicio, ya que se creó un Controlador
        que luego será usado en el Frontend
        */
        [RemoteService(IsEnabled = false)]
        // Envío de Notificaciones por Mail
        public async Task NotifyByEmailAsync(int userId, string message)
        {
            var user = await _userRepository.GetAsync(userId);

            if (user == null)
                throw new Exception("Usuario no encontrado");

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

        /*
        Se deshabilitó el servicio, ya que se creó un Controlador
        que luego será usado en el Frontend
        */
        [RemoteService(IsEnabled = false)]
        public async Task MarkReadenAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetAsync(notificationId);

            notification.MarkAsRead();

            await _notificationRepository.UpdateAsync(notification);
        }

    }
}
