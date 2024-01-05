using ConsumeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConsumeAPI.Controllers
{
    public class PersonController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5041/api");

        private readonly HttpClient _httpClient;

        public PersonController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult GET()
        {
            List<PersonModel> personModels = new List<PersonModel>();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Person/Get").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
                var dataOfObject = jsonObject.data;
                var extractedData = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                personModels = JsonConvert.DeserializeObject<List<PersonModel>>(extractedData);
            }
            return View("PersonList", personModels);
        }

        [HttpGet]
        public IActionResult Delete(int PersonID)
        {
            HttpResponseMessage response = _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Person/Delete/{PersonID}").Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Person Deleted Successfully";
            }
            return RedirectToAction("GET");
        }

        [HttpPost]
        public async Task<IActionResult> Save(PersonModel personModel)
        {
            try
            {
                MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
                multipartFormDataContent.Add(new StringContent(personModel.Name), "Name");
                multipartFormDataContent.Add(new StringContent(personModel.Email), "Email");
                multipartFormDataContent.Add(new StringContent(personModel.Contact), "Contact");
                if (personModel.PersonID == 0)
                {
                    HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Person/Post", multipartFormDataContent);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Person Added Successfully";
                        return RedirectToAction("GET");
                    }
                }
                else
                {
                    HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Person/Put/{personModel.PersonID}", multipartFormDataContent);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Person Updated Successfully";
                        return RedirectToAction("GET");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error";
            }
            return RedirectToAction("GET");
        }

        [HttpGet]
        public IActionResult PersonAdd(int PersonID)
        {
            PersonModel personModels = new PersonModel();
            HttpResponseMessage response = _httpClient.GetAsync($"{_httpClient.BaseAddress}/Person/Get/{PersonID}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
                var dataOfObject = jsonObject.data;
                var extractedData = JsonConvert.SerializeObject(dataOfObject, Formatting.Indented);
                personModels = JsonConvert.DeserializeObject<PersonModel>(extractedData);
            }
            return View("PersonAddEdit", personModels);
        }

    }
}
