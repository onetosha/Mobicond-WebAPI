using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Mobicond_WebAPI.Helpers;
using Mobicond_WebAPI.Helpers.Jwt;
using Mobicond_WebAPI.Repositories.Implementations;
using Mobicond_WebAPI.Repositories.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

services.AddSingleton<DBContext>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IDepartmentRepository, DepartmentRepository>();
services.AddScoped<IEmployeeRepository, EmployeeRepository>();
services.AddScoped<IPositionRepository, PositionRepository>();
services.AddScoped<IOrganizationRepository, OrganizationRepository>();
services.AddScoped<IHierarchyRepository, HierarchyRepository>();

services.AddControllers();

services.AddSwaggerGen();

var app = builder.Build();

//Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //redirect to swagger when server started
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/index.html")
        {
            context.Response.Redirect("/swagger");
        }
        else
        {
            await next();
        }
    });
}

app.UseMiddleware<JwtSecureMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
