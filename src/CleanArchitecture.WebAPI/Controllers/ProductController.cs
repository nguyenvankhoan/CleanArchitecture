using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.DatabaseServices;
using CleanArchitecture.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;
        public ProductController(IProductService ProductService )
        {
            _ProductService = ProductService;
        }

        // We can update search criteria later
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            var result = await _ProductService.FetchProduct();
            return result;
        }

        // GET api/Product/ProductID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var result = await _ProductService.FindProduct(id);
            return result;
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<bool>> Post(Product model)
        {
            var result = await _ProductService.CreateProduct(model);
            return result;
        }

        // PUT 
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(Product model)
        {
            var result = await _ProductService.UpdateProduct(model);
            return result;
        }

        // DELETE 
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var result = await _ProductService.DeleteProduct(id);
            return result;
        }
    }
}