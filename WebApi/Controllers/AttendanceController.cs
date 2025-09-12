using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("attendance")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _service;

    public AttendanceController(IAttendanceService service)
    {
        _service = service;
    }

    [HttpGet("missing")]
    public async Task<IActionResult> GetMissing([FromQuery] DateTime date)
    {
        var result = await _service.GetMissingAsync(date);
        return Ok(result);
    }
}
