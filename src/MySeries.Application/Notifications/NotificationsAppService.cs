using Microsoft.Extensions.Configuration.UserSecrets;
using MySeries.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)
        {
            // Comprobar si el usuario está autenticado
            if (userId <= 0)
            {
                throw new Exception("Usuario no autenticado");
            }
            // Obtener las notificaciones no leidas del usuario desde el repositorio
            var notifications = await _notificationRepository.GetListAsync(n => n.UserId == userId && !n.IsRead);
            // Mapear las notificaciones a DTOs
            var notificationDtos = notifications.Select(n => new NotificationDto(userId, n.Message)
            {
                CreatedAt = n.CreatedAt
            }).ToList();
            // Devolver la lista de DTOs de notificaciones
            return notificationDtos;
        }


        public async Task SendNotificationAsync(int userId, string message)
        {
            // Traer el usuario desde el repositorio
            var user = await _userRepository.GetAsync(userId);
            // Verificar si el usuario existe
            if (user == null)
                throw new Exception("Usuario no encontrado");
            // Verificar si el usuario permite notificaciones por app
            if (!user.NotificationsByApp)
                throw new Exception("El usuario no permite notificaciones por app");
            // Crear una nueva notificación usando el método constructor
            var notification = new Notification(userId, message);
            // Insertar la notificación en el repositorio
            await _notificationRepository.InsertAsync(notification);
        }

        public async Task NotifyByEmailAsync(int userId, string message)
        {
            // Traer el usuario desde el repositorio
            var user = await _userRepository.GetAsync(userId);
            // Verificar si el usuario existe
            if (user == null)
                throw new Exception("Usuario no encontrado");
            // Verificar si el usuario permite notificaciones por email
            if (!user.NotificationsByEmail)
                throw new Exception("El usuario no permite notificaciones por email");
            // Verificar si el usuario tiene un email registrado
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("El usuario no tiene un email registrado");
            // Enviar el email usando el servicio de email
            await _emailSender.SendAsync(
                user.Email,
                "Nueva Notificación",
                message
            );
        }

        public async Task MarkReadenAsync(int notificationId)
        {
            // Traer la notificación desde el repositorio
            var notification = await _notificationRepository.GetAsync(notificationId);
            // Marcar la notificación como leida con el método correspondiente
            notification.MarkAsRead();
            // Actualizar la notificación en el repositorio
            await _notificationRepository.UpdateAsync(notification);
        }

    }
}
