using DapperAspNetCore.Entities;
using DapperAspNetCore.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperAspNetCore.Controllers
{
    [ApiController]
    [Route("api/Products")]
    public class ProductsController(IProductRepository _repository) : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repository.GetAllAsync();


            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

            var newId = await _repository.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = newId },product);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            product.Id = id;

            var roeaffected = await _repository.UpdateAsync(id, product);
            if (roeaffected == 0)
            { return BadRequest(); }

            return Ok("Product updated successfully!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var roeaffected = await _repository.DeleteAsync(id);
            if (roeaffected == 0)
            { return BadRequest(); }

            return Ok("Product Deleted successfully!");

        }


        [HttpPost(Name ="CreateRange")]
        public async Task<IActionResult> CreateRange(IEnumerable<Product> products)
        {
            bool success = await _repository.CreateBulkAsync(products);
            if(!success)
                return BadRequest("Something went wrong!");
            return Ok("Products created successfully!");
        }
    }
}

