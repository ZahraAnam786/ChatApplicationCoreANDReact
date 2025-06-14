using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplicationCoreANDReact.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllAsync());


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _productService.GetAsync(id));


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO pDTO)
        {
            await _productService.AddAsync(pDTO);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateRecordDTO pDTO)
        {
            await _productService.UpdateAsync(pDTO);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }
    }
}
