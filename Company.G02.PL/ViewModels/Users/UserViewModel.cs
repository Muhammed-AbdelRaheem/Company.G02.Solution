using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.ViewModels.Users
{
	public class UserViewModel
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
        [RegularExpression("^\\+[1-9]{1}[0-9]{1,14}$", ErrorMessage = "Its Invalid Must Be Start With Country Code And Max 14 Number ,Ex : +211111111111111")]
        public string PhoneNumber { get; set; }
		public IEnumerable<string>? Roles { get; set; }

	}
}
