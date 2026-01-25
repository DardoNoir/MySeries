using Volo.Abp.Application.Dtos;

namespace MySeries.Usuarios
{
    public class CreateUsuarioDto
    {
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public bool NotificationsByEmail { get; set; }
        public bool NotificationsByApp { get; set; }
    }

    public class UsuarioDto : EntityDto<int>
    {
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public RolUsuarioDto Rol { get; set; }

        public bool NotificationsByEmail { get; set; }
        public bool NotificationsByApp { get; set; }
    }
}
