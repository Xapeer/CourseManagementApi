using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("payments")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentController(IStudentService service)
    {
        _service = service;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _service.GetAllAsync();
        return Ok(payments);
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? course, [FromQuery] decimal? minPayment)
    {
        var result = await _service.SearchAsync(name, course, minPayment);
        return Ok(result);
    }
}