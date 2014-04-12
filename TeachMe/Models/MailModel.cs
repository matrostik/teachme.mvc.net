using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    public class MailModel
    {
        [Required(ErrorMessage = "* From required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string From { get; set; }
        public string To { get; set; }
        [Required(ErrorMessage = "* Subject required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "* Body required")]
        public string Body { get; set; }
    }
}