using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;
using Service.Services;
using System.Threading.Tasks;

namespace PartnerFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Authorize]
    public class NotificationsController : CommonBaseController
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        public NotificationsController(INotificationService notificationService, IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var notifications = await _notificationService.GetAll(userId);
            return Ok(notifications);
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var userId = GetUserId();
            var count = await _notificationService.Count(userId);
            return Ok(count);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _notificationService.RemoveOne(id);
            await _unitOfWork.Commit();
            return Ok(new { result = true });
        }

        [HttpPost("{id}/mark-view")]
        public async Task<IActionResult> MarkView(int id)
        {
            var noti = await _notificationService.MarkView(id);
            await _unitOfWork.Commit();
            noti.IsViewed = true;
            return Ok(noti);
        }
    }
}