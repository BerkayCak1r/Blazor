using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Northwind.API.Data; // ← NorthwindContext burada
//using Northwind.API.Models; // ← Category modeli burada
using Northwind.Core.Entities;
using Northwind.Data;


namespace Northwind.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public CategoriesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }
  }
}