using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class HomeIndexViewModel
    {
        [Required]
        public string Username { get; set; }
    }
}
