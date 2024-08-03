using Microsoft.AspNetCore.Identity;

namespace AuthApp2.Model
{
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; }

	}
}
