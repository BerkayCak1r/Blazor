using System.Net.Http.Json;
using BlazorApp1.ViewModels;

namespace BlazorApp1.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<OrderViewModel>> GetOrdersAsync(
            int page, int pageSize,
            string? search = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? country = null,
            string? city = null)
        {
            var query = $"api/orders?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrWhiteSpace(search))
                query += $"&search={Uri.EscapeDataString(search)}";
            if (startDate != null)
                query += $"&startDate={startDate:yyyy-MM-dd}";
            if (endDate != null)
                query += $"&endDate={endDate:yyyy-MM-dd}";
            if (!string.IsNullOrWhiteSpace(country))
                query += $"&country={Uri.EscapeDataString(country)}";
            if (!string.IsNullOrWhiteSpace(city))
                query += $"&city={Uri.EscapeDataString(city)}";

            var response = await _httpClient.GetFromJsonAsync<PagedResult<OrderViewModel>>(query);
            return response ?? new PagedResult<OrderViewModel>();
        }
    }
}
