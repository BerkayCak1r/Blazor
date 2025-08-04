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
        // + startDate, endDate, country, city filtreleri eklendi
        [HttpGet]
        public async Task<ActionResult<PagedResult<OrderViewModel>>> GetOrders(
            [FromQuery] string? search = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? country = null,
            [FromQuery] string? city = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var query = _context.Orders
                .Include(o => o.Employee)
                .AsQueryable();

            // Arama filtresi (CustomerID + EmployeeName)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.Trim().ToLower();

                query = query.Where(o =>
                    (o.CustomerID != null && o.CustomerID.Trim().ToLower().Contains(loweredSearch)) ||
                    (o.Employee != null && (o.Employee.FirstName + " " + o.Employee.LastName).ToLower().Contains(loweredSearch))
                );

                // DateTime? parsedDate = null;
                //string[] formats = { "d.M.yyyy", "dd.MM.yyyy", "yyyy-MM-dd", "yyyy.MM.dd" };

                // if (DateTime.TryParseExact(search, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempDate))
                //     parsedDate = tempDate;
                //  else if (DateTime.TryParse(search, out var fallbackDate))
                //
                // var loweredSearch = search.Trim().ToLower();

                // query = query.Where(o =>
                //    (o.CustomerID != null && o.CustomerID.Trim().ToLower().Contains(loweredSearch)) ||
                //    (o.Employee != null && (o.Employee.FirstName + " " + o.Employee.LastName).ToLower().Contains(loweredSearch)) ||
                //    (parsedDate != null && EF.Functions.DateDiffDay(o.OrderDate, parsedDate.Value) == 0)
                //);
            }

            // Tarih aralığı filtresi
            if (startDate != null)
                query = query.Where(o => o.OrderDate >= startDate);
            if (endDate != null)
                query = query.Where(o => o.OrderDate <= endDate);

            // Ülke filtresi
            if (!string.IsNullOrWhiteSpace(country))
                query = query.Where(o => o.ShipCountry != null && o.ShipCountry.ToLower() == country.ToLower());

            // Şehir filtresi
            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(o => o.ShipCity != null && o.ShipCity.ToLower() == city.ToLower());

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

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Employee)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Sipariş verisi eksik.");

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderID }, order);
        }

        // PUT: api/orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            if (id != updatedOrder.OrderID)
                return BadRequest("ID uyuşmazlığı.");

            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null)
                return NotFound();

            _context.Entry(existingOrder).CurrentValues.SetValues(updatedOrder);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Veritabanı güncelleme hatası.");
            }

            return NoContent();
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
