using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RannaTask.DAL.Repositories.Notifications;
using RannaTask.DAL.UnitOfWorks;
using RannaTask.Entities.Common;
using RannaTask.Entities.Entities;

namespace RannaTask.Business.Notifications;

public class NotificationManager : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public NotificationManager(INotificationRepository notificationRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NotificationDto> CreateAsync(CreateNotificationDto request)
    {
        var notification = _mapper.Map<Notification>(request);
        notification.IsRead = false;

        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<NotificationDto>(notification);
    }

    public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)
    {
        var notifications = await _notificationRepository
            .Where(n => n.UserId == userId || n.UserId == null)
            .OrderByDescending(n => n.Created)
            .Take(50)
            .ToListAsync();

        return _mapper.Map<List<NotificationDto>>(notifications);
    }

    public async Task<List<NotificationDto>> GetAdminNotificationsAsync()
    {
        var notifications = await _notificationRepository
            .Where(n => n.UserId == null) // Broadcast notifications
            .OrderByDescending(n => n.Created)
            .Take(50)
            .ToListAsync();

        return _mapper.Map<List<NotificationDto>>(notifications);
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _notificationRepository
            .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
            .CountAsync();
    }

    public async Task<NoContent> MarkAsReadAsync(int notificationId)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification is null)
            return new NoContent("Bildirim bulunamadı");

        notification.IsRead = true;
        _notificationRepository.Update(notification);
        await _unitOfWork.SaveChangesAsync();

        return new NoContent("Bildirim okundu olarak işaretlendi");
    }

    public async Task<NoContent> MarkAllAsReadAsync(int userId)
    {
        var notifications = await _notificationRepository
            .Where(n => (n.UserId == userId || n.UserId == null) && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            _notificationRepository.Update(notification);
        }

        await _unitOfWork.SaveChangesAsync();
        return new NoContent("Tüm bildirimler okundu");
    }
}
