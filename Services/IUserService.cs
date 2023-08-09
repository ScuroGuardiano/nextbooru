namespace UltraHornyBoard.Services;

public interface IUserService
{
    Task<Models.User> CreateUser(Dto.UserRegisterRequest userData);
    Task<Models.User?> GetUserById(Guid id);
    Task<Models.User> MakeAdmin(string username);
}
