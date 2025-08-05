using ECart.Helpers;
using ECart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ECart.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("getallproducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _context.Product.ToList();

                if (products.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(products);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("getproduct/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = _context.Product.Find(id);

                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(product);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("addproduct")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewProduct(Product product)
        {
            try
            {
                _context.Add(product);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("updateproduct/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct(Product model)
        {
            try
            {
                if (model == null || model.Id == 0)
                {
                    return BadRequest();
                }

                var product = _context.Product.Find(model.Id);

                if (product == null)
                {
                    return NotFound();
                }

                product.ProductName = model.ProductName;
                product.Quantity = model.Quantity;
                product.Price = model.Price;

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("deleteproduct/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _context.Product.Find(id);

                if (product == null)
                {
                    return NotFound();
                }

                _context.Product.Remove(product);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

