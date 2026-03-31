using Microsoft.AspNetCore.Mvc;
using OpMetrics.Core.DTOs.Requests;
using OpMetrics.Core.Services;

namespace OpMetrics.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QualidadeController : ControllerBase   
{
    private readonly IQualidadeService _service;

    public QualidadeController(IQualidadeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("linha/{linha}")]
    public async Task<IActionResult> GetByLinha(string linha)
    {
        var result = await _service.GetByLinhaAsync(linha);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQualidadeRequest request)
    {
        var result = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateQualidadeRequest request)
    {
        var result = await _service.UpdateAsync(id, request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        if (result == false) return NotFound();
        return NoContent();
    }
}
