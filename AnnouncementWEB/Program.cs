using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


var baseUrl = builder.Configuration["ApiSettings:BaseUrl"]
              ?? "https://noticeboard-api-oleh.azurewebsites.net/";
if (!baseUrl.EndsWith("/")) baseUrl += "/";


builder.Services.AddHttpClient("AnnouncementApi", client =>
{
    client.BaseAddress = new Uri(baseUrl);
 
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
 //   options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
 //   options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

    googleOptions.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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
    name: "default",
    pattern: "{controller=Announcements}/{action=Index}/{id?}");

app.Run();