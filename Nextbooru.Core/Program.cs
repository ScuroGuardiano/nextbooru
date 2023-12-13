using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Options;
using SGLibCS.Utils.Environment;
using Nextbooru.Core.Filters;
using Nextbooru.Core.Models;
using Nextbooru.Auth;
using Nextbooru.Core;
using Nextbooru.Core.Services;
using Nextbooru.Shared.Storage;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

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
builder.Services.AddSingleton<ImageConvertionService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<ImageVotingService>();
builder.Services.AddScoped<TagsService>();
builder.Services.AddScoped<MinimalQueringImageService>();

// HOLY FUCK, WHY IT'S ENABLED BY DEFAULT XD
ValidatorOptions.Global.LanguageManager.Enabled = false;
builder.Services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());
builder.Services.AddFluentValidationAutoValidation();

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
app.Use((ctx, next) => {
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
    var config = app.Services.GetService<IOptions<AppSettings>>()!.Value;
    Console.WriteLine(string.Join(", ", config.Images.Thumbnails.Widths.Select(x => x.ToString())));
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
