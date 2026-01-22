using MySeries.Usuarios;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Application.Usuarios
{
    public class UsuariosAppService : ApplicationService, IUsuariosAppService
    {
        private readonly IRepository<Usuario, int> _userRepository;

        public UsuariosAppService(IRepository<Usuario, int> userRepository)
        {
            _userRepository = userRepository;
        }

        // 1️⃣ Crear y guardar usuario
        public async Task<UsuarioDto> crearUsuarioAsync(CreateUsuarioDto input)
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
                Password = input.Password, // ⚠️ luego lo hasheamos
                NotificationsByEmail = input.NotificationsByEmail,
                NotificationsByApp = input.NotificationsByApp
            };

            // Guardar
            await _userRepository.InsertAsync(user, autoSave: true);

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
                throw new Exception("Credenciales inválidas");

            var user = await _userRepository.FirstOrDefaultAsync(
                u => u.UserName == userName && u.Password == password
            );

            if (user == null)
                throw new Exception("Usuario o contraseña incorrectos");

            return new UsuarioDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NotificationsByEmail = user.NotificationsByEmail,
                NotificationsByApp = user.NotificationsByApp
            };
        }
    }
}
