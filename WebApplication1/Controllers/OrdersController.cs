using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Entities;
using Northwind.Data;
using WebApplication1.Models;
using WebApplication1.Models.Orders;
using System.Globalization;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public OrdersController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/orders?search=abc&page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<PagedResult<OrderViewModel>>> GetOrders(
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Orders
                .Include(o => o.Employee)
                .AsQueryable();

            // Arama filtresi
            if (!string.IsNullOrWhiteSpace(search))
            {
                DateTime? parsedDate = null;
                string[] formats = { "d.M.yyyy", "dd.MM.yyyy", "yyyy-MM-dd", "yyyy.MM.dd" };

                if (DateTime.TryParseExact(search, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempDate))
                    parsedDate = tempDate;
                else if (DateTime.TryParse(search, out var fallbackDate))
                    parsedDate = fallbackDate;

                var loweredSearch = search.Trim().ToLower();

                query = query.Where(o =>
                    (o.CustomerID != null && o.CustomerID.Trim().ToLower().Contains(loweredSearch)) ||
                    (o.Employee != null && (o.Employee.FirstName + " " + o.Employee.LastName).ToLower().Contains(loweredSearch)) ||
                    (parsedDate != null && EF.Functions.DateDiffDay(o.OrderDate, parsedDate.Value) == 0)
                );
            }



            var totalCount = await query.CountAsync();

            var pagedData = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderViewModel
                {
                    OrderID = o.OrderID,
                    CustomerID = o.CustomerID,
                    EmployeeID = o.EmployeeID,
                    EmployeeName = o.Employee != null ? o.Employee.FirstName + " " + o.Employee.LastName : null,
                    OrderDate = o.OrderDate,
                    Freight = o.Freight,
                    ShipCity = o.ShipCity,
                    ShipCountry = o.ShipCountry
                })
                .ToListAsync();

            var result = new PagedResult<OrderViewModel>
            {
                Items = pagedData,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }
    }
}
