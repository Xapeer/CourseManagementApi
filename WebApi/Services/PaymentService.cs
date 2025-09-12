using Dapper;
using WebApi.Data.Entities;

namespace WebApi.Services;

public class PaymentService : IPaymentService
{
    private readonly DbContext _dbContext;

    public PaymentService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Payment>> GetAllAsync()
    {
        var sql = @"
            select 
                s.fullname as studentname,
                g.groupname,
                c.title as coursetitle,
                p.amount,
                p.paidat
            from payments p
            join studentgroups sg on sg.id = p.studentgroupid
            join students s on s.id = sg.studentid
            join groups g on g.id = sg.groupid
            join courses c on c.id = g.courseid
        ";

        using var conn = _dbContext.CreateConnection();
        var payments = await conn.QueryAsync<Payment>(sql);
        return payments.ToList();
    }
    
    
}