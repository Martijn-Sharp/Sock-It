using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class ChatSubmitModel
    {
        public string UserName { get; set; }
        
        public string Message { get; set; }
    }
}
