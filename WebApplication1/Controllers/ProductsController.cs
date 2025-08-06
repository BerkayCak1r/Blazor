using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Entities;
using Northwind.Data;
using WebApplication1.Models;
using WebApplication1.Models.Products;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public ProductsController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts()//ProductViewModel ile döndürüyoruz ki ImageUrl ve CategoryName gibi ekstra alanlar da gelsin
        {
            return await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductViewModel
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    CategoryID = p.CategoryID,
                    CategoryName = p.Category != null ? p.Category.CategoryName : null,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<ProductViewModel>>> GetProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ServiceResponse<Product>
                {
                    Error = "Geçerli bir ürün ID'si girilmelidir."
                });
            }

            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductID == id);

                if (product == null)
                {
                    return NotFound(new ServiceResponse<Product>
                    {
                        Error = "Ürün bulunamadı"
                    });
                }

                return Ok(new ServiceResponse<Product>
                {
                    Data = product
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new ServiceResponse<Product>
                {
                    Error = "Teknik bir sorun oluştu"
                });
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                ProductName = request.ProductName,
                UnitPrice = request.UnitPrice,
                UnitsInStock = request.UnitsInStock,
                CategoryID = request.CategoryID
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
                
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
