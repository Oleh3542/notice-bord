using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

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
    googleOptions.ClientId = "130134358293-uhch1gqj0qgoat8jp57sjvi25aife639.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
   
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