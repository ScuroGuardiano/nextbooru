using Microsoft.AspNetCore.Mvc;

namespace Nextbooru.Core.Controllers;

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
        return new Dto.AppInfo()
        {
            Version = configuration["AppInfo:Version"],
            Name = configuration["AppInfo:Name"],
            Author = configuration["AppInfo:Author"],
            AuthorUrl = configuration["AppInfo:AuthorURL"]
        };
    }
}
