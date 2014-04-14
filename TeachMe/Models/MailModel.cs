using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    public class MailModel
    {
        [Required(ErrorMessage = "* יש להכניס דוא\"ל")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "דוא\"ל לא חוקי")]
        public string From { get; set; }
        public string To { get; set; }
        [Required(ErrorMessage = "* יש להכניס נושא")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "* יש להכניס תוכן")]
        public string Body { get; set; }
    }
}