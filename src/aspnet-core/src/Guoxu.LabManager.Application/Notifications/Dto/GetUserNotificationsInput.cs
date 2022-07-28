using System;
using Abp.Notifications;
using Guoxu.LabManager.Dto; 
namespace Guoxu.LabManager.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}