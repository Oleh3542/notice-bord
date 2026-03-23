using AnnouncementAPI.Entities;
using AnnouncementAPI.DTOs;

namespace AnnouncementAPI.Services
{
    public interface IAnnouncementService
    {
        Task<IEnumerable<Announcement>> GetAllAsync();
        Task<Announcement> GetByIdAsync(int id);
        Task<int> CreateAsync(AnnouncementDto dto);
        Task UpdateAsync(int id, AnnouncementDto dto);
        Task DeleteAsync(int id);
    }
}