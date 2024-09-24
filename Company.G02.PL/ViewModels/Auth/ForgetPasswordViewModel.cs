using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.ViewModels.Auth
{
	public class ForgetPasswordViewModel
	{

	

		[Required(ErrorMessage = "EmailAddress Is Required !!")]
		[EmailAddress(ErrorMessage = "EmailAddress Is Not Vailed !!")]
		public string Email { get; set; }

	
	}
}
