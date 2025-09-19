using Dapper;
using WebApi.Data.Entities;

namespace WebApi.Services;

public class StudentService(DbContext _dbContext, IWebHostEnvironment environment) : IStudentService
{
    
    public async Task<List<AddStudentDto>> GetAllAsync()
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
        var students = await conn.QueryAsync<AddStudentDto>(sql);
        return students.ToList();
    }
    
    public async Task<List<AddStudentDto>> SearchAsync(string? name, string? course, decimal? minPayment)
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
        var list = await conn.QueryAsync<AddStudentDto>(sql, new { name, course, minPayment });
        return list.ToList();
    }
    
    
    public async Task<GetStudentDto> Add(AddStudentDto addStudentDto)
    {
        string sql;
        int resultId;
        string filename = string.Empty;
        if (addStudentDto.ProfilePicture is null)
        {
            sql =
                @"insert into students (FullName,Phone) 
                    values(@FullName,@Phone) returning id";
            resultId = await _dbContext.CreateConnection().ExecuteScalarAsync<int>(sql, addStudentDto);   
        }
        else
        {
            filename = Guid.NewGuid().ToString() + Path.GetExtension(addStudentDto.ProfilePicture.FileName);
            var savedFile = await CreateImageFile(addStudentDto.ProfilePicture,filename);
            if (savedFile == false)
                throw new Exception("File not saved");
           
            sql = @"insert into students (FullName,Phone) 
                    values(@FullName,@Phone) returning id";
            resultId = await _dbContext.CreateConnection().ExecuteScalarAsync<int>(sql, new
            {
                fullname = addStudentDto.FullName,
                phone = addStudentDto.Phone,
                profilepicture = filename,
            });

        }

        return new GetStudentDto()
        {
            Id = resultId,
            FullName = addStudentDto.FullName,
            Phone = addStudentDto.Phone,
            ProfilePicture = addStudentDto.ProfilePicture != null ? GenerateFileLink(filename) : string.Empty
        };

    }
    private string GenerateFileLink(string filename)
    {
        var path = Path.Combine("http://localhost:5255", "images", filename);
        return path;
    }

    private async Task<bool> CreateImageFile(IFormFile file, string filename)
    {
        var filedirectory = Path.Combine(environment.WebRootPath, "images", filename);
        await using (var stream = new FileStream(filedirectory, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return true;
    }
    

}