using BlazorKeycloak.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using System.Net;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    authOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    authOptions.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddOpenIdConnect(oidcOptions =>
    {
		oidcOptions.Authority = "http://192.168.50.166:8080/realms/myrealm01";
		oidcOptions.ClientId = "webapp01";
		oidcOptions.ClientSecret = "OyudSWgC8RXznbhcYOsk0P1sL4ObyHka";
		oidcOptions.ResponseType = "code";
		oidcOptions.SaveTokens = true;
		oidcOptions.Scope.Add("profile");
		oidcOptions.RequireHttpsMetadata = false;
    })
	.AddCookie();

var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
