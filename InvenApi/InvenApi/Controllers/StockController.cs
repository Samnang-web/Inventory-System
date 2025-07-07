using InvenApi.DTOs;
using InvenApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _service;

        public StockController(IStockService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stock = await _service.GetAllAsync();
            return Ok(stock);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _service.GetByIdAsync(id);
            if (stock == null) NotFound();
            return Ok(stock);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockDto dto)
        {
            var stockId = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = stockId }, new { id = stockId });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return Ok(new { success = true });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return Ok(new { success = false });
        }
    }
}
