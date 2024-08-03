using System.ComponentModel.DataAnnotations;

namespace AuthApp2.DTO
{
	public class LogInDTO
	{
		[Required]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
