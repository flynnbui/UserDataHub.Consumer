using System.ComponentModel.DataAnnotations;

namespace UserDataHub.Consumer.Core.Entites
{
    public class UserData
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string IdCard { get; set; }

        [Required]
        public DateTime IssueDateIdCard { get; set; }
    }
}
