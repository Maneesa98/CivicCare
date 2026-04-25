using CivicCare.Infrastructure.Data;
using CivicCare.Infrastructure.Services;
using CivicCare.Api.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using MediatR;

using CivicCare.Application.Requests;
using CivicCare.Application.Contracts;
using CivicCare.Application.Validators;
using CivicCare.Application.Behaviors;
using FluentValidation;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

//
// -------------------- SERVICES --------------------
//

// Controllers
builder.Services.AddControllers();

//
// Swagger
//
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CivicCare.Api",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

//
// Http Context
//
builder.Services.AddHttpContextAccessor();

//
// Database
//
builder.Services.AddDbContext<CivicCareDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<CivicCareDbContext>());

//
// Email Service
//
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

//
// MediatR
//
builder.Services.AddMediatR(typeof(CreateServiceRequest).Assembly);

//
// FluentValidation — REGISTER VALIDATORS
//
builder.Services.AddValidatorsFromAssemblyContaining<
    CreateServiceRequestValidator>();

//
// FluentValidation — PIPELINE BEHAVIOR
//
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>)
);

//
// JWT Authentication
//
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]
                )
            )
        };
    });

//
// Authorization
//
builder.Services.AddAuthorization();


//
// -------------------- APP --------------------
//

var app = builder.Build();

//
// Global Exception Middleware
//
app.UseMiddleware<ExceptionMiddleware>();

//
// Swagger
//
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//
// Authentication / Authorization
//
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();