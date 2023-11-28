using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Portfolio.Backend.Common.Data.Repository;
using Portfolio.Backend.Services.Interfaces;
using Portfolio.Backend.Services.Implementations;
using Portfolio.Backend.Common.Helpers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Portfolio.Backend.Middleware.Requirements;
using Portfolio.Backend.Middleware.Handlers;
using Portfolio.Backend;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfiguration(new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build());

builder.Services.Configure<FormOptions>(o => o.MultipartBodyLengthLimit = 1024 * 1024 * 1024);

builder.Services.AddCors(o => o.AddPolicy(name: "DebugAppPolicy", policy => { policy.WithOrigins(builder.Configuration.GetValue<string>("AllowedOrigin") ?? "", "http://localhost:3000", "http://127.0.0.1:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); }));

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddDbContext<AppDatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"), b => b.MigrationsAssembly("Portfolio.Backend.Application"));
});

builder.Services.AddSingleton<IMailService, MailService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IAuthorizationHandler, JwtCookieRequirementHandler>();
builder.Services.AddSingleton<DatabaseHelper>();
builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler, JwtCookieAuthorizationMiddlewareResultHandler>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "backend", Version = "v1" });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.Cookie.Name = "Auth";
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .AddRequirements(new JwtCookieRequirement())
    .Build();
});

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;
IdentityModelEventSource.LogCompleteSecurityArtifact = true;

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend v1"));
}
app.UseCors("DebugAppPolicy");
// app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();