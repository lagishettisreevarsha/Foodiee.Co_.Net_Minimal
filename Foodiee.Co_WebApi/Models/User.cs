using System.ComponentModel.DataAnnotations;

namespace Foodiee.Co_WebApi.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } 

        [Required]
        public string Email { get; set; } 

        [Required]
        public string Password { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Url]
        public string? PictureUrl { get; set; }

        public ICollection<Food> Foods { get; set; } = new List<Food>();
    }
}
