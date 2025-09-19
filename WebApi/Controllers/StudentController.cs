using Microsoft.AspNetCore.Mvc;
using QRCoder;
using WebApi.Data.Entities;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("students")]
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
    
    [HttpGet("{id}/qrcode")]
    public async Task<IActionResult> GetQrCode([FromRoute] int id)
    {
        var url = $"http://localhost:5255/students/{id}";
        
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrData);
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        return File(qrCodeBytes, "image/png");
    }
    
    [HttpPost("add-student")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Add([FromForm]AddStudentDto student)
    {
        return Ok(await _service.Add(student));
    }
}