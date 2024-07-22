using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
services.AddScoped<IJobTitleRepository, JobTitleRepository>();
services.AddScoped<IOrganizationRepository, OrganizationRepository>();
services.AddScoped<IHierarchyRepository, HierarchyRepository>();
services.AddScoped<IRouteRepository, RouteRepository>();

services.AddControllers();

services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
