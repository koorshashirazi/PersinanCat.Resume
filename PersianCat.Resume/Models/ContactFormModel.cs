using System.ComponentModel.DataAnnotations;

namespace PersianCat.Resume.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "NameRequired")]
        [MinLength(2, ErrorMessage = "NameTooShort")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "SubjectRequired")]
        [MinLength(10, ErrorMessage = "SubjectTooShort")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "MessageRequired")]
        [MinLength(10, ErrorMessage = "MessageTooShort")]
        public string Message { get; set; } = string.Empty;
    }
}
