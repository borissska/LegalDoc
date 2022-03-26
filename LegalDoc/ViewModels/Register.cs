using System.ComponentModel.DataAnnotations;

namespace LegalDoc.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пустая строка. Пожалуйста, введите почтовый адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пустая строка. Пожалуйста, введите номер телефона")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Пустая строка. Пожалуйста, введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
