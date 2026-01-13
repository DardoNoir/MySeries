using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Identity.Client;
using Volo.Abp.Domain.Entities;

namespace MySeries.Usuarios
{
    public class Usuario : AggregateRoot<int>
    {
        public Guid? UserId { get; set; }
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }

        public bool NotificationsByEmail { get; set; }
        public bool NotificationsByApp { get; set; }


        public bool PrefiereNotificacionEmail() => NotificationsByEmail;
        public bool PrefiereNotificacionApp() => NotificationsByApp;

    }
}