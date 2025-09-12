using WebApi.Data.Entities;

namespace WebApi.Services;

public interface ICourseService
{
    public  Task<List<CourseStats>> GetStatsAsync();
}