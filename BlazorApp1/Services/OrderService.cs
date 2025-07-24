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

        public async Task<PagedResult<OrderViewModel>> GetOrdersAsync(int page, int pageSize, string? search = null)
        {
            var query = $"api/orders?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
            {
                query += $"&search={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetFromJsonAsync<PagedResult<OrderViewModel>>(query);
            return response ?? new PagedResult<OrderViewModel>();
        }
    }
}
