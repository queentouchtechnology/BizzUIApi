using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BizzUIBlazor.Components.Models;


namespace BizzUIBlazor.Components.Services
{
    public class ProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiResponse<List<Product>>> GetProducts()
        {
            return await httpClient.GetFromJsonAsync<ApiResponse<List<Product>>>("products");
        }
      

        public async Task<Product> GetProductById(int id)
        {
            return await httpClient.GetFromJsonAsync<Product>($"products/get/{id}");
        }


        // public async Task<string> SubmitForm(Product product)
        // {
        //     var response = await httpClient.PostAsJsonAsync("Products", product);

        //     if (response.IsSuccessStatusCode)
        //     {
        //         return await response.Content.ReadAsStringAsync();
        //     }
        //     else
        //     {
        //         return $"Error: {response.StatusCode}";
        //     }
        // }


        public async Task<Product> AddProduct(Product product)
        {
            var response = await httpClient.PostAsJsonAsync("products", product);
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var response = await httpClient.PutAsJsonAsync($"products/{product.Id}", product);
            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var response = await httpClient.DeleteAsync($"products/delete/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}