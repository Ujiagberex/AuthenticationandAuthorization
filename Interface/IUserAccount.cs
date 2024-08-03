using AuthApp2.DTO;
using static AuthApp2.DTO.ServiceResponses;

namespace AuthApp2.Interface
{
	public interface IUserAccount
	{
		Task<GeneralResponse> CreateAccount(UserDTO userDTO);
		Task<LoginResponse> LoginAccount(LogInDTO logInDTO);
	}
}
