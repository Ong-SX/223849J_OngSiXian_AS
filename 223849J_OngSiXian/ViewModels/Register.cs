using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace _223849J_OngSiXian.ViewModels
{
	public class Register
	{
		[Required]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string LastName { get; set; }
		
		[Required]
		[DataType(DataType.EmailAddress)]
		[RegularExpression(@"^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$", ErrorMessage ="Email address is invalid")]
		public string Email { get; set; }
		
		[Required]
		[DataType(DataType.PhoneNumber)]
        [MinLength(8, ErrorMessage = "Mobile number should have 8 numbers only")]
        [MaxLength(8, ErrorMessage = "Mobile number should have 8 numbers only")]
        public string PhoneNumber { get; set; }

		[Required]
		[DataType(DataType.CreditCard)]
		[RegularExpression(@"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$", ErrorMessage = "Credit card number is invalid")]
		public string CreditCard { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string BillingAddress { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string ShippingAddress { get; set; }

		[Required]
        public string Photo { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[MinLength(12, ErrorMessage = "Enter at least a 12 characters password")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}$",
		ErrorMessage = "Passwords must be at least 12 characters long and contain at least an upper case letter, lower case letter, digit and a symbol")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
	    public string ConfirmPassword { get; set; }
	}
}
