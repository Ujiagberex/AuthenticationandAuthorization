using AuthApp2.DTO;
using AuthApp2.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp2.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController(IUserAccount userAccount) : ControllerBase
	{
		[HttpPost("RegisterUser")]

		public async Task<IActionResult> RegisterUser(UserDTO userDTO)
		{
			var Response = await userAccount.CreateAccount(userDTO);
			return Ok(Response);
		}

		[HttpPost("UserLogIn")]

		public async Task<IActionResult> UserLogIn([FromBody] LogInDTO logInDTO)
		{
			var Response = await userAccount.LoginAccount(logInDTO);
			return Ok(Response);
		}
	}
}
