using Project.Models;
using Project.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Project.Data;
using Project.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddScoped<DbService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
ConfigureDB(builder.Services);
var app = builder.Build();
app.UseRouting();
app.UseStaticFiles();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();

static void ConfigureDB(IServiceCollection services)
{
    var config = services.BuildServiceProvider().GetService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    services.AddDbContext<ApplicationContext>(x => x.UseSqlServer(connectionString));
}
public partial class Program { };