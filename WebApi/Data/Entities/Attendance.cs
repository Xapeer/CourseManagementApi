namespace WebApi.Data.Entities;

public class Attendance
{
    public string StudentName { get; set; }
    public string GroupName { get; set; }
    public string CourseTitle { get; set; }
    public DateTime LessonDate { get; set; }
}