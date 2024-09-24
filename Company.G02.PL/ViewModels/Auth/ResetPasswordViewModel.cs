using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.ViewModels.Auth
{
	public class ResetPasswordViewModel
	{


		[DataType(DataType.Password)]
		[MaxLength(16)]
		[MinLength(4)]
		public string Password { get; set; }

		[Required(ErrorMessage = "ConfirmPassword Is Required !!")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Confirmed Password Not Match Password")]
		public string ConfirmPassword { get; set; }
	}
}
