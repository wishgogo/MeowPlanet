using MeowPlanet.Models;
using System.ComponentModel.DataAnnotations;

namespace MeowPlanet.ViewModels
{
    public class RegisterModel : Member
    {
        public new int MemberId { get; set; }
        [Required(ErrorMessage = "請輸入有效的Email")]
        [EmailAddress(ErrorMessage = "請輸入有效的Email")]
        public new string Email { get; set; } = null!;
        [Required(ErrorMessage = "請輸入有效的密碼")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "密碼必須是6-10位")]
        public new string Password { get; set; } = null!;
        [Required(ErrorMessage = "請輸入有效的手機號碼")]
        [RegularExpression(@"^09[0-9]{8}$",ErrorMessage =  "手機號碼格式不正確")]
        public new string Phone { get; set; } = null!;
        [Required(ErrorMessage = "請輸入有效的名字")]
        public new string Name { get; set; } = null!;
        public new string? Photo { get; set; }
    }
}
