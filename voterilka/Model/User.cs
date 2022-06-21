using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace voterilka.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [MinLength(5, ErrorMessage = "Логин должен быть от 5 до 20 символов")]
        [MaxLength(20, ErrorMessage = "Логин должен быть от 5 до 20 символов")]
        public string Username { get; set; }
        //public int? RoleId { get; set; }
        //public Role Role { get; set; }
        public string PicUrl { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(8, ErrorMessage = "Пароль должен быть от 8 до 30 символов")]
        [MaxLength(30, ErrorMessage = "Пароль должен быть от 8 до 30 символов")]
        public string Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirmation { get; set; }

        public ICollection<Variant> Variants { get; set; }
        public User()
        {
            Variants = new List<Variant>();
            PicUrl = "../images/blank-profile-picture.png";
        }
    }
}
