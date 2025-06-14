using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using ChatApplicationCoreANDReact;
using ChatApplicationCoreANDReact.Middleware;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


//user blew two from one
//DependencyInjection.RegisterServices(builder.Services);
//OR
builder.Services.AddApplicationServices();


builder.Services.AddDbContext<ApplicationDBContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddSignalR();

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:51051") // Your React app's port
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ✅ This must be included

       //  policy.WithOrigins("http://localhost:51051").AllowAnyHeader().AllowAnyMethod();
    });
});


// ✅ Add Swagger
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// 1. Development-only tools
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi(); // Optional — only if you're using NSwag
}

// 2. Serve static files (before routing to avoid conflicts)
app.UseStaticFiles();


// Serve "Files" folder statically
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Files")),
    RequestPath = "/Files"
});

// 4. Custom middleware for global exception handling
app.UseMiddleware<ExceptionMiddleware>();

// 5. HTTPS redirection
app.UseHttpsRedirection();

// 6. Routing must be before Authentication and Authorization
app.UseRouting();


// 3. CORS — should be placed before anything that uses it (like controllers)
app.UseCors("AllowReactApp");

// 7. Authentication before Authorization
app.UseAuthentication();
app.UseAuthorization();

// 8. Map controllers or endpoints
app.MapControllers();

// ✅ 4. Map Hub WITH CORS
app.MapHub<ChatHub>("/chathub").RequireCors("AllowReactApp");

app.Run();

