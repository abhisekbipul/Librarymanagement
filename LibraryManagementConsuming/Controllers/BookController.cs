using LibraryManagementConsuming.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace LibraryManagementConsuming.Controllers
{
    public class BookController : Controller
    {
        HttpClient client;
        public BookController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
        }

        public IActionResult Index()
        {
            List<Book> bookList = new List<Book>();
            string url = "https://localhost:7211/api/Books/GetAllBooks";

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                bookList = JsonConvert.DeserializeObject<List<Book>>(jsonData);
            }

            return View(bookList);
        }

        public IActionResult AddBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Book data is null.");
            }

            string url = "https://localhost:7211/api/Books/AddBook";
            var jsonData = JsonConvert.SerializeObject(newBook);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage result = client.PostAsync(url, stringContent).Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(newBook);
        }

        public IActionResult DeleteBook(int id)
        {
            string url = $"https://localhost:7211/api/Books/DeleteBook/{id}";

            HttpResponseMessage result = client.DeleteAsync(url).Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        public IActionResult IssueBook()
        {
            // Get the list of users who have library cards for selection in the view
            List<LibraryCard> libraryCardList = new List<LibraryCard>();
            string url = "https://localhost:7211/api/LibraryCards/GetAllLibraryCards";

            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                libraryCardList = JsonConvert.DeserializeObject<List<LibraryCard>>(jsonData);
            }

            ViewBag.LibraryCards = new SelectList(libraryCardList, "UserId", "CardNumber");
            return View();
        }

        [HttpPost]
        public IActionResult IssueBook(int bookId, int userId)
        {
            // Ensure the user has a valid library card before issuing a book
            string cardUrl = $"https://localhost:7211/api/LibraryCards/GetLibraryCardByUserId/{userId}";
            HttpResponseMessage cardResponse = client.GetAsync(cardUrl).Result;

            if (!cardResponse.IsSuccessStatusCode)
            {
                return BadRequest("User does not have a valid library card.");
            }

            // Issue the book to the user
            string url = $"https://localhost:7211/api/Books/IssueBook/{bookId}/{userId}";
            HttpResponseMessage result = client.PostAsync(url, null).Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return BadRequest("Failed to issue the book.");
        }


    }
}
