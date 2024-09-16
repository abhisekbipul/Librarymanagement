using LibraryManagementConsuming.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace LibraryManagementConsuming.Controllers
{
    public class LibraryCardController : Controller
    {
        HttpClient client;

        public LibraryCardController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
        }

        // Displays all library cards
        public IActionResult Index()
        {
            List<LibraryCard> libraryCardList = new List<LibraryCard>();
            string url = "https://localhost:7211/api/LibraryCards/GetAllLibraryCards";

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                libraryCardList = JsonConvert.DeserializeObject<List<LibraryCard>>(jsonData);
            }

            return View(libraryCardList);
        }

        // Add a new library card
        public IActionResult IssueLibraryCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IssueLibraryCard(LibraryCard newLibraryCard)
        {
            if (newLibraryCard == null)
            {
                return BadRequest("Library card data is null.");
            }

            string url = "https://localhost:7211/api/LibraryCards/IssueLibraryCard";
            var jsonData = JsonConvert.SerializeObject(newLibraryCard);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage result = client.PostAsync(url, stringContent).Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(newLibraryCard);
        }
    }
}
