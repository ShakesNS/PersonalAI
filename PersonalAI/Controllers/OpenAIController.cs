using Microsoft.AspNetCore.Mvc;
using PersonalAI.Models;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PersonalAI.Controllers
{
    public class OpenAIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenAIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async  Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> OpenAIResponse()
        {
            var httpClient = _httpClientFactory.CreateClient("OpenAI");

            var requestContent = new
            {
                prompt = "Translate the following English text to French: 'Hello, world.'",
                max_tokens = 50
            };

            var content = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);
                ViewBag.GeneratedText = result.choices[0].text;
            }
            else
            {
                ViewBag.Error = $"Hata kodu: {response.StatusCode}";
                ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync();
            }

            return View();

        }

        public async Task<IActionResult> AICreateImage()
        {
            var httpClient = _httpClientFactory.CreateClient("Dall-E");

            var requestContent = new
            {
                model = "dall-e-3",
                prompt = "a white siamese cat",
                size = "1024x1024",
                quality = "standard",
                n = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonData = JObject.Parse(responseContent);
                //var result = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);
                ViewBag.ImageUrl= jsonData["data"][0]["url"].ToString();
            }
            else
            {
                ViewBag.Error = $"Hata kodu: {response.StatusCode}";
                ViewBag.ErrorMessage = await response.Content.ReadAsStringAsync();
            }

            return View();

        }

    }
}
