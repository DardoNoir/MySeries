using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace MySeries.Notifications
{
    public class Notification : AggregateRoot<int>
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        public Notification(int usuarioid)
        {
            CreatedAt = DateTime.UtcNow;
            IsRead = false;
            UserId = usuarioid;
            Message = string.Empty;
        }
    }
}