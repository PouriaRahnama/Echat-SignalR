using Echat.Application.Services.Chats.ChatGroups;
using Echat.Application.Services.Chats;
using Echat.Application.Services.Roles;
using Echat.Application.Services.Users.UserGroups;
using Echat.Application.Services.Users;
using Echat.Domain.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Echat.UI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var services = builder.Services;
var Configuration = builder.Configuration;


services.AddControllersWithViews();
services.AddDbContext<EchatContext>(option =>
{
    option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
});

services.AddScoped<IChatService, ChatService>();
services.AddScoped<IChatGroupService, ChatGroupService>();

services.AddScoped<IRoleService, RoleService>();

services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserGroupService, UserGroupService>();

services.AddSignalR();
services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(option =>
{
    option.LoginPath = "/auth";
    option.LogoutPath = "/auth/Logout";
    option.ExpireTimeSpan = TimeSpan.FromDays(7);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chat");

app.Run();
