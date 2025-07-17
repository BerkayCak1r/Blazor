using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using WebApplication1.Data; // ← NorthwindContext burada
//using WebApplication1.Models; // ← Category modeli burada
using Northwind.Core.Entities;
using Northwind.Data;


namespace WebApplication1.Controllers
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
