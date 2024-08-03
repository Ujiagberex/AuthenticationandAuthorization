using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthApp2.DTO;
using AuthApp2.Interface;
using AuthApp2.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using static AuthApp2.DTO.ServiceResponses;

namespace AuthApp2.Repository
{
	public class AccountRepo(
		UserManager<ApplicationUser> userManager,
		RoleManager<IdentityRole> roleManager,
		IConfiguration config) : IUserAccount
	{
		public async Task<GeneralResponse> CreateAccount(UserDTO userDTO)
		{
			if (userDTO == null) return new GeneralResponse(false, "Model is empty");
			var newUser = new ApplicationUser()
			{
				FullName = userDTO.FullName,
				Email = userDTO.Email,
				PasswordHash = userDTO.Password,
				UserName = userDTO.Email
			};
			var user = await userManager.FindByEmailAsync(newUser.Email);
			if (user != null) return new GeneralResponse(false, "User already exist");

			var createUSer = await userManager.CreateAsync(newUser!, userDTO.Password);
			if (!createUSer.Succeeded) return new GeneralResponse(false, "Error Occured");

			//ASsign default roles: Admin is the first register
			var checkAdmin = await roleManager.FindByNameAsync("Admin");
			if (checkAdmin == null)
			{
				await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
				await userManager.AddToRoleAsync(newUser, "Admin");
				return new GeneralResponse(true, "Account Created");
			}
			else
			{
				var checkUser = await roleManager.FindByNameAsync("User");
				if (checkUser == null)
					await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

				await userManager.AddToRoleAsync(newUser, "User");
				return new GeneralResponse(true, "Account Created");

			}
		}

		public async Task<LoginResponse> LoginAccount(LogInDTO logInDTO)
			{
			if (logInDTO == null)
				return new LoginResponse(false, null!, "Login container is empty");

			var getUser = await userManager.FindByEmailAsync(logInDTO.Email);
			if (getUser == null)
				return new LoginResponse(false, null!, "User not found");

			bool checkedPassword = await userManager.CheckPasswordAsync(getUser, logInDTO.Password);
			if (!checkedPassword)
				return new LoginResponse(false, null!, "Invalid Email/Password");

			var getUserRole = await userManager.GetRolesAsync(getUser);
			var userSession = new UserSession(getUser.Id, getUser.FullName, getUser.Email, getUserRole.First());
			string token = GenerateToken(userSession);
			return new LoginResponse(true, token!, "Login Completed");
		}

		private string GenerateToken(UserSession user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var userClaims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.FullName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
			};

			var token = new JwtSecurityToken(
				issuer: config["Jwt:Issuer"],
				audience: config["Jwt:Audience"],
				claims: userClaims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: credentials
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

	}
}
