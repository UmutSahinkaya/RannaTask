using RannaTask.DAL.Contexts;
using RannaTask.Entities.Entities;

namespace RannaTask.DAL.Repositories.Notifications;

public interface INotificationRepository : IGenericRepository<Notification, int>
{
}

public class NotificationRepository : GenericRepository<Notification, int>, INotificationRepository
{
    private readonly AppDbContext _context;
    
    public NotificationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
