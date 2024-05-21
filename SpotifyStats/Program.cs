using SpotifyAPI.Web;
using SpotifyStats.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddAuthorization();
services.AddAuthentication();
services.AddControllers();
services.AddControllersWithViews();
services.AddSingleton<SpotifyClientFactory>();
services.AddScoped<SpotifyClient>(provider =>
{
    var factory = provider.GetRequiredService<SpotifyClientFactory>();
    return factory.CreateClient();
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/auth");
    }
    else
    {
        await next();
    }
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Music}/{action=Index}/{id?}");

app.Run();