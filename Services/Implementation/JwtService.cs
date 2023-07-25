namespace UltraHornyBoard.Services.Implementation;

public class JwtService : IJwtService
{

    private IConfiguration configuration;

    public JwtService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string SignToken(string subject)
    {
        throw new NotImplementedException();
    }

    public string GetKey()
    {
        return "";
    }
}