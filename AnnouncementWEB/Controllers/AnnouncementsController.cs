using AnnouncementWEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;

namespace AnnouncementWEB.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AnnouncementsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _clientFactory.CreateClient("AnnouncementApi");

            try
            {
                var response = await client.GetAsync("api/announcements");

                if (response.IsSuccessStatusCode)
                {

                    var announcements = await response.Content.ReadFromJsonAsync<IEnumerable<AnnouncementViewModel>>();
                    return View(announcements ?? new List<AnnouncementViewModel>());
                }

   
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Помилка API: {response.StatusCode}. {error}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Критична помилка зв'язку: {ex.Message}";
            }

            return View(new List<AnnouncementViewModel>());
        }

        [Authorize]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            model.CreatedDate = DateTime.Now;

            var client = _clientFactory.CreateClient("AnnouncementApi");
            var response = await client.PostAsJsonAsync("api/announcements", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Сервер повернув помилку: {errorContent}");
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return BadRequest("Некоректний ID.");

            var client = _clientFactory.CreateClient("AnnouncementApi");
            string requestUrl = $"api/announcements/{id}";

            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadFromJsonAsync<AnnouncementViewModel>();
                if (item != null) return View(item);
            }


            var errorBody = await response.Content.ReadAsStringAsync();
            return Content($"Діагностика: Статус {response.StatusCode}. " +
                           $"Переконайтеся, що ви додали [HttpGet(\"{{id}}\")] в API контролер і зробили успішний Publish API. " +
                           $"Відповідь сервера: {errorBody}");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnnouncementViewModel model)
        {

            if (id != model.Id) return BadRequest($"Невідповідність ID: URL={id}, Model={model.Id}");

            if (!ModelState.IsValid) return View(model);

            var client = _clientFactory.CreateClient("AnnouncementApi");
            var response = await client.PutAsJsonAsync($"api/announcements/{id}", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Помилка оновлення: {error}");
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient("AnnouncementApi");
            var response = await client.DeleteAsync($"api/announcements/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Не вдалося видалити оголошення.";
            return RedirectToAction(nameof(Index));
        }
    }
}