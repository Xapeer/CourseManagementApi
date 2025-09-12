using WebApi.Data.Entities;

namespace WebApi.Services;

public interface IPaymentService
{
    public Task<List<Payment>> GetAllAsync();
}