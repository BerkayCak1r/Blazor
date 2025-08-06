using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Core.Entities;
using Northwind.Data;
using Northwind.API.Models;
using Northwind.API.Models.Orders;

namespace Northwind.API.Controllers
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

        // GET: api/orders?search=abc&page=1&pageSize=10&startDate=2024-01-01&country=USA&city=Seattle
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
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .AsQueryable();

            // Arama filtresi (CustomerID, CustomerName, EmployeeName)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.Trim().ToLower();

                query = query.Where(o =>
                    (o.CustomerID != null && o.CustomerID.Trim().ToLower().Contains(loweredSearch)) ||
                    (o.Customer != null && o.Customer.ContactName != null &&
                        o.Customer.ContactName.ToLower().Contains(loweredSearch)) ||
                    (o.Employee != null &&
                        (o.Employee.FirstName + " " + o.Employee.LastName).ToLower().Contains(loweredSearch))
                );
            }

            // Tarih aralığı filtresi
            if (startDate != null)
                query = query.Where(o => o.OrderDate >= startDate);
            if (endDate != null)
                query = query.Where(o => o.OrderDate <= endDate);

            // Ülke filtresi
            if (!string.IsNullOrWhiteSpace(country))
                query = query.Where(o => o.ShipCountry != null &&
                                         o.ShipCountry.ToLower() == country.ToLower());

            // Şehir filtresi
            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(o => o.ShipCity != null &&
                                         o.ShipCity.ToLower() == city.ToLower());

            var totalCount = await query.CountAsync();

            var pagedData = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderViewModel
                {
                    OrderID = o.OrderID,
                    CustomerID = o.CustomerID ?? string.Empty,

                    CustomerName = o.Customer != null ? o.Customer.ContactName : string.Empty,
                    EmployeeID = o.EmployeeID,
                    EmployeeName = o.Employee != null
                        ? o.Employee.FirstName + " " + o.Employee.LastName
                        : string.Empty,
                    OrderDate = o.OrderDate,
                    Freight = o.Freight,
                    ShipCity = o.ShipCity ?? string.Empty,
                    ShipCountry = o.ShipCountry ?? string.Empty,
                    TotalAmount = (o.OrderDetails != null && o.OrderDetails.Any()
                        ? o.OrderDetails.Sum(od =>
                            (decimal)od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
                        : 0) + (o.Freight ?? 0)

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
        public async Task<ActionResult<OrderViewModel>> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
                return NotFound();

            var vm = new OrderViewModel
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID ?? string.Empty,
                
                CustomerName = order.Customer?.ContactName ?? string.Empty,
                EmployeeID = order.EmployeeID,
                EmployeeName = order.Employee != null
                    ? $"{order.Employee.FirstName} {order.Employee.LastName}"
                    : string.Empty,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                ShipCity = order.ShipCity ?? string.Empty,
                ShipCountry = order.ShipCountry ?? string.Empty,
                Freight = order.Freight,
                TotalAmount = (order.OrderDetails != null && order.OrderDetails.Any()
                    ? order.OrderDetails.Sum(od =>
                        (decimal)od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
                    : 0) + (order.Freight ?? 0),

                OrderDetails = order.OrderDetails.Select(od => new OrderDetailItem
                {
                    ProductID = od.ProductID,
                    
                    ProductName = od.Product != null ? od.Product.ProductName : string.Empty,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Discount = od.Discount,
                    ImageUrl = od.Product != null ? od.Product.ImageUrl : null

                }).ToList()
            };

            return Ok(vm);
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
