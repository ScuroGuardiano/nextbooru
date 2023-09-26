using Microsoft.AspNetCore.Mvc;

namespace UltraHornyBoard.Core.Controllers;

[ApiController]
[Route("")]
public class HornyController : ControllerBase
{
    private readonly IConfiguration configuration;

    public HornyController(IConfiguration config)
    {
        configuration = config;
    }

    [HttpGet(Name = "GetAppInfo")]
    public Dto.AppInfo Get()
    {
        return new()
        {
            Version = configuration["AppInfo:Version"],
            Name = configuration["AppInfo:Name"],
            Author = configuration["AppInfo:Author"],
            AuthorURL = configuration["AppInfo:AuthorURL"]
        };
    }
}
