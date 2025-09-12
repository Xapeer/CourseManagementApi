namespace WebApi.Data.Entities;

public class Payment
{
    public string StudentName { get; set; }
    public string GroupName { get; set; }
    public string CourseTitle { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
}
