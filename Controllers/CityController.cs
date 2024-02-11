using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class CityController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5041/api");

        private readonly HttpClient _httpClient;

        public CityController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        #region City List
        [HttpGet]
        public IActionResult GET()
        {
            List<CityModel> cityModels = new List<CityModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/City/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
                var dataOfObject = jsonObject.data;
                var extractedData = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                cityModels = JsonConvert.DeserializeObject<List<CityModel>>(extractedData);
            }
            return View("CityList", cityModels);
        }
        #endregion
    }
}
