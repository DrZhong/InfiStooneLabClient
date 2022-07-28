using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using Abp.Web.Security.AntiForgery;
using Guoxu.LabManager.Controllers;
using Microsoft.AspNetCore.Http;
using System.IO;
using Abp.IO.Extensions;
using Guoxu.LabManager.Storage;
using System;
using Abp.Authorization;
using Abp.Auditing;
using System.Linq;

namespace Guoxu.LabManager.Web.Host.Controllers
{
    public class HomeController : LabManagerControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public HomeController(INotificationPublisher notificationPublisher, ITempFileCacheManager tempFileCacheManager)
        {
            _notificationPublisher = notificationPublisher;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public IActionResult Index()
        {
            return Redirect("/swagger");
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }
        [DisableAuditing]
        [AbpAuthorize]
        public string UploadExcel([FromForm(Name ="file")] IFormFile excel)
        {
            //var profilePictureFile = Request.Form.Files.First();
            var stream = new MemoryStream();
            excel.CopyTo(stream);
            stream.Seek(0, SeekOrigin.Begin);   
            var  fileBytes = stream.GetAllBytes();
            var token = Guid.NewGuid().ToString();
            _tempFileCacheManager.SetFile(token, new TempFileCacheItem()
            {
                Bytes = fileBytes,
                FileName = excel.FileName
            });

           return token;
        }
    }
}
