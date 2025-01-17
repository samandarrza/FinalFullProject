using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EtradeDbContext>(opt =>{
    opt.UseSqlServer("Server=DESKTOP-HO9CBPN\\SQLEXPRESS;Database=EtradeDb; Trusted_Connection=TRUE");
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddDefaultTokenProviders().AddEntityFrameworkStores<EtradeDbContext>();

builder.Services.AddScoped<LayoutService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = context =>
    {
        if (context.HttpContext.Request.Path.Value.StartsWith("/admin"))
        {
            var redirectPath = new Uri(context.RedirectUri);
            context.Response.Redirect("/admin/account/login" + redirectPath.Query);
        }
        else
        {
            var redirectPath = new Uri(context.RedirectUri);
            context.Response.Redirect("/account/login" + redirectPath.Query);
        }

        return Task.CompletedTask;
    };
});
builder.Services.AddAuthentication().AddGoogle(opt =>{
    opt.ClientId = "639788485977-df4n5uov911384j47u85c2v54blit7ah.apps.googleusercontent.com";
    opt.ClientSecret = "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";
    opt.SignInScheme = IdentityConstants.ExternalScheme;
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
       name: "areas",
       pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
   );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
