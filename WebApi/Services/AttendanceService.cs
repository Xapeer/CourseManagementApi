using Dapper;
using WebApi.Data.Entities;

namespace WebApi.Services;

public class AttendanceService : IAttendanceService
{
    private readonly DbContext _dbContext;
    
    public AttendanceService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Attendance>> GetMissingAsync(DateTime date)
    {
        var sql = @"
            select 
                s.fullname as studentname,
                g.groupname as groupname,
                c.title as coursetitle,
                a.lessondate as lessondate
            from attendance a
            join studentgroups sg on sg.id = a.studentgroupid
            join students s on s.id = sg.studentid
            join groups g on g.id = sg.groupid
            join courses c on c.id = g.courseid
            where a.ispresent = false
              and a.lessondate = @date
            order by s.fullname;
        ";

        using var conn = _dbContext.CreateConnection();
        var list = await conn.QueryAsync<Attendance>(sql, new { date });
        return list.ToList();
    }
}