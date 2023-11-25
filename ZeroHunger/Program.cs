using Microsoft.EntityFrameworkCore;
using ZeroHunger.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add session configuration
builder.Services.AddDistributedMemoryCache(); // Use an in-memory cache for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(3);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// Use session middleware
app.UseSession();

// middleware for session to redirect from Landing page or signin page
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value.ToLower();
    var isAuthenticated = context.Session.GetString("IsAuthenticated");
    Console.WriteLine($"Requested Path: {context.Request.Path}");

    if (path == "/account/login" || path == "/account/signin")
    {
        // Check if the session variable indicates successful sign-in
        
        if (!string.IsNullOrEmpty(isAuthenticated) && bool.TryParse(isAuthenticated, out var isAuthenticatedValue) && isAuthenticatedValue)
        {
            context.Response.Redirect("/Home/Index");
            return;
        }
    }
    else if (path != "/" && path != "/account/signup")
    {
        if (string.IsNullOrEmpty(isAuthenticated))
        {
            context.Response.Redirect("/");
            return;
        }
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Landing}/{action=Index}/{id?}");

app.Run();
