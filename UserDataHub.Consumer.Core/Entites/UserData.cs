using System.ComponentModel.DataAnnotations;

namespace UserDataHub.Consumer.Core.Entites
{
    public class UserData
    {
        [Required]
        public required string Address { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public required string FullName { get; set; }

        [Required]
        public required string Gender { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string Status { get; set; }

        [Required]
        public required string IdCard { get; set; }

        [Required]
        public DateTime IssueDateIdCard { get; set; }
    }
}
