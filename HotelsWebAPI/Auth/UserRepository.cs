namespace HotelsWebAPI.Auth;

public class UserRepository : IUserRepository
{
    private IEnumerable<UserDto> Users =>
    [
        new UserDto("John", "1234"),
        new UserDto("Deer", "12345")
    ];

    public UserDto GetUser(UserModel userModel)
    {
        return Users.FirstOrDefault(u =>
                   string.Equals(u.UserName, userModel.UserName) &&
                   string.Equals(u.Password, userModel.Password)) ??
               throw new Exception();
    }
}