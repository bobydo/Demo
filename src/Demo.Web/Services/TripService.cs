using Demo.Web.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace Demo.Web.Services
{
    public class TripsService
    {
        private readonly HttpClient _http;

        public TripsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Trip_DTO>> GetTripsAsync()
        {
            try
            {
                var trips = await _http.GetFromJsonAsync<List<Trip_DTO>>("api/Trips");
                return trips ?? new List<Trip_DTO>();
            }
            catch (Exception)
            {
                // Optionally log the error
                return new List<Trip_DTO>();
            }
        }
    }
}
