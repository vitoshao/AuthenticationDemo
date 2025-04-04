using System.ComponentModel.DataAnnotations;

namespace Demo.DAL
{
    public partial class Users
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Account { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string Password { get; set; } = null!;
    }
}
