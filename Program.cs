using Microsoft.Extensions.Options;
using UltraHornyBoard;
using UltraHornyBoard.Filters;
using UltraHornyBoard.Services;
using UltraHornyBoard.Services.Implementation;
using UltraHornyBoard.Helpers.Environment;
using Models = UltraHornyBoard.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.Configure<AppSettings>(builder.Configuration);
EnviromentVariablesMapper.MapVariables(AppSettings.EnvMappings);
builder.Configuration.AddEnvironmentVariables(AppSettings.EnvPrefix);

builder.Services.AddOptions<AppSettings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddDbContext<Models.HornyContext>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddControllers(options => {
    options.Filters.Add<HttpResponseExceptionFilter>();
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