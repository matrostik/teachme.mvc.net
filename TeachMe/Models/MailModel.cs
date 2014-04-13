using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    public class MailModel
    {
        [Required(ErrorMessage = "* יש להכניס דוא\"ל")]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$",ErrorMessage="* כתובת דוא\"ל לא חוקית ")]
        public string From { get; set; }
        public string To { get; set; }
        [Required(ErrorMessage = "* יש להכניס נושא")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "* יש להכניס תוכן")]
        public string Body { get; set; }
    }
}