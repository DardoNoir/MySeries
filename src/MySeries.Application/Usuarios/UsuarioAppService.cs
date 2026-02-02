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

        /*
        Se deshabilitó el servicio, ya que se creó un Controlador
        que luego será usado en el Frontend
        */
        [RemoteService(IsEnabled = false)]
        // Crear y guardar usuario
        public async Task<UsuarioDto> CrearUsuarioAsync(CreateUsuarioDto input)
        {
            if (string.IsNullOrWhiteSpace(input.UserName))
                throw new Exception("El nombre de usuario es obligatorio");

            if (string.IsNullOrWhiteSpace(input.Password))
                throw new Exception("La contraseña es obligatoria");

            var existingUser = await _userRepository.FirstOrDefaultAsync(
                u => u.UserName == input.UserName
            );

            if (existingUser != null)
                throw new Exception("El nombre de usuario ya existe");

            var user = new Usuario
            {
                UserName = input.UserName,
                Email = input.Email,
                Password = input.Password,
                NotificationsByEmail = input.NotificationsByEmail,
                NotificationsByApp = input.NotificationsByApp,
                Rol = RolUsuario.User // Se lo crea siempre como Usuario, para tener Rol Admin --> cambiar en la DB
            };

            await _userRepository.InsertAsync(user, autoSave: true);

            if (user.NotificationsByApp)
            {
                await _notificationService.SendNotificationAsync(
                    user.Id,
                    $"👋 Bienvenido {user.UserName} a MySeries!"
                );
            }

            if (user.NotificationsByEmail)
            {
                await _notificationService.NotifyByEmailAsync(
                    user.Id,
                    "¡Bienvenido a MySeries! 🎬🍿"
                );
            }

            return new UsuarioDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NotificationsByEmail = user.NotificationsByEmail,
                NotificationsByApp = user.NotificationsByApp
            };
        }

        // Traer usuario 
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
