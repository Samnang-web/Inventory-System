using InvenApi.DTOs;
using InvenApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cat = await _service.GetAllAsync();
            return Ok(cat);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _service.GetByIdAsync(id);
            if (cat == null) NotFound();
            return Ok(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var catId = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = catId }, new { id = catId });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (!result) NotFound();
            return Ok(new {success = true});
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) NotFound();
            return Ok(new { success = true });
        }
    }
}
