using macrix_api.Models;

const string corsPolicyDev = "corsPolicyDev";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyDev, policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<PeopleContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
