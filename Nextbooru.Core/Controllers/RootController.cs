using Microsoft.AspNetCore.Mvc;
using Nextbooru.Core.Dto.Responses;

namespace Nextbooru.Core.Controllers;

[ApiController]
[Route("")]
public class RootController : ControllerBase
{
    private readonly IConfiguration configuration;

    public RootController(IConfiguration config)
    {
        configuration = config;
    }

    [HttpGet(Name = "GetAppInfo")]
    public AppInfo Get()
    {
        return new AppInfo()
        {
            Version = configuration["AppInfo:Version"],
            Name = configuration["AppInfo:Name"],
            Author = configuration["AppInfo:Author"],
            AuthorUrl = configuration["AppInfo:AuthorURL"]
        };
    }
}
