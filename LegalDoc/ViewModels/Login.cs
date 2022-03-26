using System.ComponentModel.DataAnnotations;

namespace LegalDoc.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Данные об аккаунте были введены неправильно")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
