using WebApi.Data.Entities;

namespace WebApi.Services;

public interface IStudentService
{
    public Task<List<AddStudentDto>> GetAllAsync();
    public Task<List<AddStudentDto>> SearchAsync(string? name, string? course, decimal? minPayment);

    public Task<GetStudentDto> Add(AddStudentDto addStudentDto);
}