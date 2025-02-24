using System.ComponentModel.DataAnnotations;

namespace Demo1.Models.ViewModels
{
    public class VmLogin
    {
        [Required(ErrorMessage = "使用者帳號 是必要項")]
        [Display(Name = "使用者帳號")]
        [StringLength(50, ErrorMessage = "登入帳號最多50個字")]
        public string Account { get; set; } = "";

        [Required(ErrorMessage = "使用者密碼 是必要項")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "密碼最多50個字")]
        [Display(Name = "使用者密碼")]
        public string Password { get; set; } = "";

        public string? ReturnUrl { get; set; }
    }
}
