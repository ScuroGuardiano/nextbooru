using Microsoft.Extensions.Options;
using SGLibCS.Utils.Environment;
using UltraHornyBoard.Core.Filters;
using UltraHornyBoard.Core.Models;
using UltraHornyBoard.Auth;
using UltraHornyBoard.Core;

var builder = WebApplication.CreateBuilder(args);
EnviromentVariablesMapper.MapVariables(AppSettings.EnvMappings);
builder.Configuration.AddEnvironmentVariables(AppSettings.EnvPrefix);

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContext<HornyContext>();
builder.Services.AddSGAuthentication<HornyContext>();

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

app.UseAuthentication();
app.UseAuthorization();

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
