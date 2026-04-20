using System.ComponentModel.DataAnnotations;

namespace BniSittingManager.Models
{
    public class RegisterViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "User Name Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string UserType { get; set; }

        public string ReferenceType { get; set; }

        public int? DoctorId { get; set; }

        public int? NurseId { get; set; }
    }
}
