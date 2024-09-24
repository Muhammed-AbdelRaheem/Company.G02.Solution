using Company.G02.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.G02.PL.Helper
{
	public static class EmailSettings
	{

		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("muhammedbika22@gmail.com", "dyxghpdkgxpgbycb");

			client.Send("muhammedbika22@gmail.com",email.To,email.Subject,email.Body);
			//dyxghpdkgxpgbycb

		}
	}
}
