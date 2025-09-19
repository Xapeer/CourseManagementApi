namespace WebApi.Data.Entities;

public class AddStudentDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public IFormFile ProfilePicture { get; set; }
}