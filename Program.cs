using exam_system.Domain.Entities.Diplomas;
using exam_system.Domain.Entities.Identity;
using exam_system.Features.Shared;
using exam_system.Persistence;
using exam_system.Persistence.Context;
using exam_system.Persistence.DataAccess;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//Adding Rate Limitting Service
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10;
    });
});
//Options Pattern
var optionsPattern = builder.Configuration.GetSection("JWT").Get<OptionsPattern>();
builder.Services.AddSingleton(optionsPattern);
//Allow DependencyInjection for Idenitty
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
//Jwt AuthenticationService
builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = optionsPattern.Issuer,
        ValidateAudience = true,
        ValidAudience = optionsPattern.Audeience,
        IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(optionsPattern.Key))

    };
}
    );


var app = builder.Build();

// Seed Database automatically on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        await AppDbContextSeed.SeedAsync(context, logger,roleManager,userManager);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during database migration/seeding.");
    }
}

// Enable Swagger UI in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examination System API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Adding RateLimiter Middleware
app.UseRateLimiter();

// Test Minimal API Endpoint to verify database access and generic repository
app.MapGet("/api/test/diplomas", async (IGenericRepository<Diploma> diplomaRepo, CancellationToken ct) =>
{
    var diplomas = await diplomaRepo.GetAll()
        .Select(d => new
        {
            d.Id,
            d.Title,
            d.Description,
            QuizzesCount = d.Quizzes.Count,
            EnrollmentsCount = d.Enrollments.Count,
            d.CreatedAt
        })
        .ToListAsync(ct);

    return Results.Ok(new
    {
        Success = true,
        Count = diplomas.Count,
        Data = diplomas
    });
})
.WithName("GetTestDiplomas")
.WithTags("Test");

app.MapControllers();

app.Run();
