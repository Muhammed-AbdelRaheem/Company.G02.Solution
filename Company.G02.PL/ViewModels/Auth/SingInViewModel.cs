using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.ViewModels.Auth
{
	public class SingInViewModel
	{
		[Required(ErrorMessage = "EmailAddress Is Required !!")]
		[EmailAddress(ErrorMessage = "EmailAddress Is Not Vailed !!")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password Is Required !!")]

		[DataType(DataType.Password)]
		[MaxLength(16)]
		[MinLength(4)]
		public string Password { get; set; }

		public bool RememberMe {  get; set; }
	}
}
