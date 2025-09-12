using Dapper;
using WebApi.Data.Entities;

namespace WebApi.Services;

public class CourseService : ICourseService
{
    private readonly DbContext _dbContext;

    public CourseService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<CourseStats>> GetStatsAsync()
    {
        var sql = @"
            select 
                c.title as CourseTitle,
                count(sg.studentid) as StudentCount,
                sum(p.amount) as TotalPayments
            from courses c
            left join groups g on g.courseid = c.id
            left join studentgroups sg on sg.groupid = g.id
            left join payments p on p.studentgroupid = sg.id
            group by c.title
        ";

        using var conn = _dbContext.CreateConnection();
        var stats = await conn.QueryAsync<CourseStats>(sql);
        return stats.ToList();
    }
}