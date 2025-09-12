using Dapper;
using WebApi.Data.Entities;

namespace WebApi.Services;

public class StudentService : IStudentService
{
    private readonly DbContext _dbContext;

    public StudentService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Student>> GetAllAsync()
    {
        var sql = @"
        select 
            s.id,
            s.fullname,
            s.phone,
            g.groupname,
            c.title as coursename
        from students s
        join studentgroups sg on sg.studentid = s.id
        join groups g on g.id = sg.groupid
        join courses c on c.id = g.courseid
        order by s.id;
    ";

        using var conn = _dbContext.CreateConnection();
        var students = await conn.QueryAsync<Student>(sql);
        return students.ToList();
    }
    
    public async Task<List<Student>> SearchAsync(string? name, string? course, decimal? minPayment)
    {
        var sql = @"
            select 
                s.fullname as fullname,
                c.title as coursetitle,
                sum(p.amount) as totalpayments
            from students s
            join studentgroups sg on sg.studentid = s.id
            join groups g on g.id = sg.groupid
            join courses c on c.id = g.courseid
            join payments p on p.studentgroupid = sg.id
            where (@name is null or s.fullname ilike '%' || @name || '%')
              and (@course is null or c.title = @course)
            group by s.id, s.fullname, c.title
            having (@minPayment is null or sum(p.amount) > @minPayment)
        ";

        using var conn = _dbContext.CreateConnection();
        var list = await conn.QueryAsync<Student>(sql, new { name, course, minPayment });
        return list.ToList();
    }
    
    
    
    

}