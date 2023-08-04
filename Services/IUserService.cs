namespace UltraHornyBoard.Services;

public interface IUserService
{
    Task<Models.User> CreateUser(Dto.UserRegisterRequest userData);
}
