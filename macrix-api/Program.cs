using System.Reflection;
using macrix_api.Models;

const string corsPolicyDev = "corsPolicyDev";


var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyDev, policy =>
    {
        policy.WithOrigins(settings.CorsHosts);
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<PeopleContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(corsPolicyDev);
app.UseAuthorization();
app.MapControllers();

app.Run();
