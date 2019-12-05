using Data.Models;

namespace Data.Repositories
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {

    }
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
