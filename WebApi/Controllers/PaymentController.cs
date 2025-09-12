using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _service;

    public PaymentController(IPaymentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _service.GetAllAsync();
        return Ok(payments);
    }
}
