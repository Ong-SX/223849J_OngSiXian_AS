using System.ComponentModel.DataAnnotations;

namespace _223849J_OngSiXian.ViewModels
{
    public class TwoFA
    {
        [Required]
        public string Code { get; set; }

        public bool RememberMe { get; set; }
    }
}
