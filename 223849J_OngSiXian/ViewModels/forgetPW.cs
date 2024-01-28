using System.ComponentModel.DataAnnotations;

namespace _223849J_OngSiXian.ViewModels
{
    public class forgetPW
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
