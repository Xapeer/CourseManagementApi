using WebApi.Data.Entities;

namespace WebApi.Services;

public interface IStudentService
{
    public Task<List<Student>> GetAllAsync();
    public Task<List<Student>> SearchAsync(string? name, string? course, decimal? minPayment);
    
    
}