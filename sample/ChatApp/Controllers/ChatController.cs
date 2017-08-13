using ChatApp.Models;
using SockIt.Abstractions;
using SockIt.ViewFeatures;

namespace ChatApp.Controllers
{
    public class ChatController : WebSocketController
    {
        public IWebSocketActionResult Submit(ChatSubmitModel model)
        {
            return View("Submit", model);
        }
    }
}
