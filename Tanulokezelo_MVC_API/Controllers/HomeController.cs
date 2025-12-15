using System.Diagnostics;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Tanulokezelo_MVC_API.Models;
using System.Text;

namespace Tanulokezelo_MVC_API.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient httpClient;
        //Http kliens API hívásokhoz
        public HomeController(IHttpClientFactory factory)
        {
            httpClient = factory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost:7077/");
            //Az alkalmazás portja
        }

        public async Task<IActionResult> Index()
        {
            //API hívás: GET /api/studentApi
            var valasz = await httpClient.GetAsync("api/studentapi");

            Console.WriteLine(valasz);

            var json = await valasz.Content.ReadAsStringAsync();

            Console.WriteLine(json);
            //API -> JSON fájl
            var lista = JsonSerializer.Deserialize<List<StudentDTO>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(lista);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentDTO s)
        {
            //c# obj => JSON
            var json = JsonSerializer.Serialize(s);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //API Hívás: POST /api/studentapi/create
            var response = await httpClient.PostAsync("/api/studentapi/create", data);

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", errorMessage);

                ViewData["Hibakód"] = errorMessage;
                return View();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int om)
        {
            await httpClient.DeleteAsync($"api/studentapi/delete/{om}");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int om)
        {
            var valasz = await httpClient.GetAsync($"api/studentapi/{om}");
            if (!valasz.IsSuccessStatusCode) return NotFound();

            var json = await valasz.Content.ReadAsStringAsync();
            var lista = JsonSerializer.Deserialize<Student>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int om)
        {
            var valasz = await httpClient.GetAsync($"api/studentapi/{om}");

            if (!valasz.IsSuccessStatusCode) return NotFound();

            var json = await valasz.Content.ReadAsStringAsync();

            var atad = JsonSerializer.Deserialize<StudentDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(atad);
        }

        [HttpPost]
        public async Task<IActionResult> Update(StudentDTO tanulo)
        {
            var json = JsonSerializer.Serialize(tanulo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            await httpClient.PutAsync($"/api/studentapi/modify/{tanulo.OMidentifier}", data);

            return RedirectToAction("Index");
        }

        public IActionResult FilterGPA()
        {
            FilterStudent atmenti = new FilterStudent();
            return View(atmenti);
        }

        [HttpPost]
        public async Task<IActionResult> FilterGPA(FilterStudent s)
        {
            var valasz = await httpClient.GetAsync($"studentapi/api/filter/{s.GPA}");

            if (!valasz.IsSuccessStatusCode) return NotFound();

            var json = await valasz.Content.ReadAsStringAsync();

            var atad = JsonSerializer.Deserialize<FilterStudent>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(atad);
        }
    }
}