using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    public class MailModel
    {
        [Required(ErrorMessage = "* From required")]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$",ErrorMessage="* Not a valid email ")]
        public string From { get; set; }
        public string To { get; set; }
        [Required(ErrorMessage = "* Subject required")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "* Body required")]
        public string Body { get; set; }
    }
}