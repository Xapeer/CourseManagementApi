using WebApi.Data.Entities;

namespace WebApi.Services;

public interface IAttendanceService
{
    public Task<List<Attendance>> GetMissingAsync(DateTime date);
}