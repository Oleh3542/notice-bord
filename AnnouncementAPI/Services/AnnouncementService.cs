using AnnouncementAPI.DTOs;
using AnnouncementAPI.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AnnouncementAPI.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly string _connectionString;

        public AnnouncementService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                return await connection.QueryAsync<Announcement>("spGetAnnouncements",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Announcement> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                const string sql = "SELECT * FROM Announcements WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Announcement>(sql, new { Id = id });
            }
        }

        public async Task<int> CreateAsync(AnnouncementDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Category = dto.Category,
                    SubCategory = dto.SubCategory,
                    Status = dto.Status,
                    CreatedDate = DateTime.Now 
                };

                return await connection.ExecuteAsync("spInsertAnnouncement",
                    parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateAsync(int id, AnnouncementDto dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Id = id,
                    Title = dto.Title,
                    Description = dto.Description,
                    Category = dto.Category,
                    SubCategory = dto.SubCategory,
                    Status = dto.Status

                };

                await connection.ExecuteAsync("spUpdateAnnouncement",
                    parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("spDeleteAnnouncement",
                    new { Id = id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}