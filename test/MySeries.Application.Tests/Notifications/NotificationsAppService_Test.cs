using MySeries.Usuarios;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Emailing;
using Xunit;
using Xunit.Sdk;

namespace MySeries.Notifications
{
    public sealed class NotificationsAppService_Tests
    {
        private readonly NotificationsAppService _service;
        private readonly IRepository<Notification, int> _notificationRepo;
        private readonly IRepository<Usuario, int> _userRepo;
        private readonly IEmailSender _emailSender;

        public NotificationsAppService_Tests()
        {
            _notificationRepo = Substitute.For<IRepository<Notification, int>>();
            _userRepo = Substitute.For<IRepository<Usuario, int>>();
            _emailSender = Substitute.For<IEmailSender>();

            _service = new NotificationsAppService(
                _notificationRepo,
                _emailSender,
                _userRepo
            );
        }

        // Test Para Verificar Notificaciones por Email según Preferencias del Usuario
        [Fact]
        public async Task ShouldEmailIfUserPrefers()
        {
            var userId = 1;
            var message = "Test de Notificación";

            var usuario = new Usuario
            {
                UserName = "UsuarioTest",
                Password = "PasswordTest",
                Email = "UsuarioMail@Example.com",
                NotificationsByEmail = true
            };

            _userRepo.GetAsync(userId).Returns(usuario);

            await _service.NotifyByEmailAsync(userId, message);

            await _emailSender.Received(1).SendAsync(
                usuario.Email,
                "Nueva Notificación",
                message
            );
        }

        // Test Para Verificar que No se Envían Notificaciones por Email si el Usuario No lo Prefiere
        [Fact]
        public async Task ShouldNotEmailIfUserDoesNotPreferException()
        {
            // Arrange
            var userId = 2;
            var message = "Test de Notificación";
            var usuario = new Usuario
            {
                UserName = "UsuarioTest2",
                Password = "PasswordTest2",
                Email = "UsuarioTest2@Example.com",
                NotificationsByEmail = false
            };

            // Act & Assert
            _userRepo.GetAsync(userId).Returns(usuario);
            await Assert.ThrowsAsync<Exception>(() =>
                _service.NotifyByEmailAsync(userId, message));


            await _emailSender.DidNotReceive().SendAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>()
            );
        }

        // Test Para Verificar que No se Envían Notificaciones por Email si el Usuario No Existe
        [Fact]
        public async Task ShouldNotFoundUserException()
        {
            //Arrange
            var userId = 3;

            _userRepo.FindAsync(userId).Returns((Usuario)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _service.NotifyByEmailAsync(userId, "Test"));

            await _emailSender.DidNotReceive().SendAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>()
            );
        }
    }
}
