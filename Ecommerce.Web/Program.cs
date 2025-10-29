using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();
app.Run();
app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();
app.Run();
