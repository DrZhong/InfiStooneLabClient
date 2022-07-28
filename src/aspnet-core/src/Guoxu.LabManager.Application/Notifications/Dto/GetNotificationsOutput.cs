using System;
using System.Collections.Generic; 
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Notifications;
using Abp.Timing;
using Newtonsoft.Json;

namespace Guoxu.LabManager.Notifications.Dto
{
    [AutoMapFrom(typeof(TenantNotification))]
    public class TenantNotificationDto :    EntityDto<Guid> 
    { /// <summary>Tenant Id.</summary>
        public int? TenantId { get; set; }

        /// <summary>Unique notification name.</summary>
        public string NotificationName { get; set; }

        /// <summary>Notification data.</summary>
        public NotificationData Data { get; set; } 

        /// <summary>Name of the entity type (including namespaces).</summary>
        public string EntityTypeName { get; set; }

        /// <summary>Entity id.</summary>
        public object EntityId { get; set; }

        /// <summary>Severity.</summary>
        public NotificationSeverity Severity { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Abp.Notifications.TenantNotification" /> class.
        /// </summary>
        public TenantNotificationDto()
        {
            this.CreationTime = Clock.Now;
        }
    }

    [AutoMapFrom(typeof(UserNotification))]
    public class UserNotificationDto : EntityDto<Guid>
    {
        public int? TenantId { get; set; }

        /// <summary>User Id.</summary>
        public long UserId { get; set; }

        /// <summary>Current state of the user notification.</summary>
        public UserNotificationState State { get; set; }

        /// <summary>The notification.</summary>
        public TenantNotificationDto Notification { get; set; }
    }

    public class GetNotificationsOutput : PagedResultDto<UserNotificationDto>
    {
        public int UnreadCount { get; set; }

        public GetNotificationsOutput(int totalCount, int unreadCount, List<UserNotificationDto> notifications)
            :base(totalCount, notifications)
        {
            UnreadCount = unreadCount;
        }
    }
}