using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new HomeIndexViewModel());
        }

        [HttpPost]
        public IActionResult Index(HomeIndexViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            return RedirectToAction("Chat", new {userId = viewModel.Username});
        }

        public IActionResult Chat(string userId)
        {
            return View(new HomeChatViewModel { UserName =  userId });
        }
    }
}
