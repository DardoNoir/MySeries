using System;
using Volo.Abp.Domain.Entities;

namespace MySeries.Notifications
{
    public class Notification : AggregateRoot<int>
    {
        public int UserId { get; private set; }
        public string? Message { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsRead { get; private set; }

        // 🔹 Constructor requerido por EF Core
        protected Notification()
        {
        }

        // 🔹 Constructor de dominio
        public Notification(int userId, string message)
        {
            UserId = userId;
            Message = message;
            CreatedAt = DateTime.UtcNow;
            IsRead = false;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
