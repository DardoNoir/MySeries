using System;

namespace MySeries.Notifications
{
    public class NotificationDto
    {
        public int UserId { get; set; }
        public bool IsRead { get; set; }  
        public DateTime CreatedAt { get; set; }
        public string? Message { get; set; }

        public NotificationDto(int userId, string? message)
        {
            UserId = userId;
            Message = message;
        }
    }
}