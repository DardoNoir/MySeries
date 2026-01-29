using MySeries.Notifications;
using MySeries.Usuarios;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Application.Usuarios
{
    public class UsuariosAppService : ApplicationService, IUsuariosAppService
    {
        private readonly IRepository<Usuario, int> _userRepository;
        private readonly NotificationsAppService _notificationService;

        public UsuariosAppService(IRepository<Usuario, int> userRepository, NotificationsAppService notificationsAppService)
        {
            _userRepository = userRepository;
            _notificationService = notificationsAppService;
        }

        // 1️⃣ Crear y guardar usuario
        [RemoteService(IsEnabled = false)]
        public async Task<UsuarioDto> CrearUsuarioAsync(CreateUsuarioDto input)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(input.UserName))
                throw new Exception("El nombre de usuario es obligatorio");

            if (string.IsNullOrWhiteSpace(input.Password))
                throw new Exception("La contraseña es obligatoria");

            // Verificar si ya existe el username
            var existingUser = await _userRepository.FirstOrDefaultAsync(
                u => u.UserName == input.UserName
            );

            if (existingUser != null)
                throw new Exception("El nombre de usuario ya existe");

            // Crear entidad
            var user = new Usuario
            {
                UserName = input.UserName,
                Email = input.Email,
                Password = input.Password,
                NotificationsByEmail = input.NotificationsByEmail,
                NotificationsByApp = input.NotificationsByApp,
                Rol = RolUsuario.User 
            };

            // Guardar
            await _userRepository.InsertAsync(user, autoSave: true);

            // 🎉 Notificación bienvenida APP
            if (user.NotificationsByApp)
            {
                await _notificationService.SendNotificationAsync(
                    user.Id,
                    $"👋 Bienvenido {user.UserName} a MySeries!"
                );
            }

            // ✉️ Notificación bienvenida EMAIL
            if (user.NotificationsByEmail)
            {
                await _notificationService.NotifyByEmailAsync(
                    user.Id,
                    "¡Bienvenido a MySeries! 🎬🍿"
                );
            }


            // Mapear a DTO
            return new UsuarioDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NotificationsByEmail = user.NotificationsByEmail,
                NotificationsByApp = user.NotificationsByApp
            };
        }

        // 2️⃣ Traer usuario para login
        public async Task<UsuarioDto> GetUsuarioAsync(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                throw new BusinessException("Credenciales inválidas");

            var user = await _userRepository.FirstOrDefaultAsync(
                u => u.UserName == userName && u.Password == password
            );

            if (user == null)
                throw new BusinessException("Usuario o contraseña incorrectos");

            return new UsuarioDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NotificationsByEmail = user.NotificationsByEmail,
                NotificationsByApp = user.NotificationsByApp,
                Rol = (RolUsuarioDto)user.Rol
            };
        }
    }
}
