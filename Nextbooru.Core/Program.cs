using Microsoft.Extensions.Options;
using SGLibCS.Utils.Environment;
using Nextbooru.Core.Filters;
using Nextbooru.Core.Models;
using Nextbooru.Auth;
using Nextbooru.Core;
using Nextbooru.Core.Services;
using Nextbooru.Shared.Storage;

var builder = WebApplication.CreateBuilder(args);
EnviromentVariablesMapper.MapVariables(AppSettings.EnvMappings);
builder.Configuration.AddEnvironmentVariables(AppSettings.EnvPrefix);

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddSGAuthentication<AppDbContext>();


builder.Services.AddSingleton<IMediaStore, LocalMediaStore>();
builder.Services.AddScoped<ImageService>();

builder.Services.AddControllers(options => {
    options.Filters.Add<HttpResponseExceptionFilter>();
    options.Filters.Add(new ValidateModelAttribute());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); FUUUUCK
app.UsePathBase(new PathString("/api"));

app.UseAuthentication();
app.UseAuthorization();

// Block using api routes without prefix.
// TODO: change later to support SPA
app.Use((HttpContext ctx, RequestDelegate next) => {
    if (ctx.Request.PathBase == string.Empty)
    {
        ctx.Response.StatusCode = 404;
        return Task.CompletedTask;
    }
    return next(ctx);
});

app.MapControllers();

try
{ 
    app.Run();
}
catch (Exception ex)
{
    if (ex is OptionsValidationException optionsValidationException)
    {
        Console.WriteLine("@@@@@@@@@@ CONFIGURATION ERROR @@@@@@@@@@");
        Console.WriteLine(optionsValidationException.Message);
    }
    else
    {
        throw;
    }
}
