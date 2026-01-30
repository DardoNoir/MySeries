using Azure.Identity;
using MySeries.Application.Usuarios;
using MySeries.Notifications;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace MySeries.Usuarios
{
    public sealed class UsuariosAppService_Test
    {
        private readonly IRepository<Usuario, int> _usuarioRepository;
        private readonly UsuariosAppService _service;
        private readonly NotificationsAppService _notificationsAppService;
        public UsuariosAppService_Test()
        {
            _usuarioRepository = Substitute.For<IRepository<Usuario, int>>();
        //    _notificationsAppService = Substitute.For<NotificationsAppService>();

            _service = new UsuariosAppService(
                _usuarioRepository, _notificationsAppService
            );
        }

        [Fact]
        public async Task ShouldCreateUsuario()
        {
            // Arrange
            var user = new CreateUsuarioDto
            {
                UserName = "testuser",
                Password = "Test@1234",
            };

            // Act
            var createdUser = await _service.CrearUsuarioAsync(user);

            // Assert
            Assert.NotNull(createdUser);
            Assert.Equal("testuser", createdUser.UserName);
        }
    }
}
