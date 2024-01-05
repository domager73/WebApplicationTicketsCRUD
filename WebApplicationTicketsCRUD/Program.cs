using System.Text;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Exceptions;
using WebApplicationTicketsCRUD.Kafka;
using WebApplicationTicketsCRUD.Repositories;
using WebApplicationTicketsCRUD.Services;
using WebApplicationTicketsCRUD.Util;
using WebApplicationTicketsCRUD.Validators;

var builder = WebApplication.CreateBuilder(args);


var producerConfiguration = new ProducerConfig();
builder.Configuration.Bind("producerconfiguration", producerConfiguration);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheUrl"];
});

builder.Services.AddSingleton<ProducerConfig>(producerConfiguration);
builder.Services.AddSingleton<RedisUtil>();
builder.Services.AddSingleton<TestKafka>();

builder.Services.AddTransient<TicketsDbContext>();

builder.Services.AddSingleton<UserValidator>();

builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<TicketTypeRepository>();
builder.Services.AddTransient<TicketRepository>();

builder.Services.AddSingleton<TicketsService>();
builder.Services.AddSingleton<TicketTypeService>();
builder.Services.AddSingleton<AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        LifetimeValidator = (before, expires, token, parameters) =>
        {
            return expires.Value >= DateTime.UtcNow;
        },
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "You api title", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors => cors
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();