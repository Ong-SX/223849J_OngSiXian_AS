using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _223849J_OngSiXian.Model
{
	public class Member : IdentityUser
	{
		[Required]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.CreditCard)]
		public string CreditCard {  get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string BillingAddress { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string ShippingAddress { get; set; }

		[Required]
		public string Photo { get; set; }
	}
}
