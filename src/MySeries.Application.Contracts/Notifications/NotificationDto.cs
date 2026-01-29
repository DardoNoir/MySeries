using System;
using Volo.Abp.Application.Dtos;

namespace MySeries.Notifications
{
    public class NotificationDto : EntityDto<int>
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