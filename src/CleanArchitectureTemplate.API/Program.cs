using Hangfire;
using HangfireBasicAuthenticationFilter;
using CleanArchitectureTemplate.API.Middleware;
using CleanArchitectureTemplate.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwagger();

builder.Services.AddServicesDependancies(builder.Configuration);
builder.Services.AddPersistenceDependancies(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddHangeFire(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
	Authorization = new[]
	{
		new HangfireCustomBasicAuthenticationFilter { User = "cleanarch", Pass = "cleanarch@Pass" }
	}
});

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
