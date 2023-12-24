using WebApplicationTicketsCRUD.Db.DbConnector;
using WebApplicationTicketsCRUD.Exceptions;
using WebApplicationTicketsCRUD.Services;
using WebApplicationTicketsCRUD.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheUrl"];
});

builder.Services.AddTransient<TicketsDbContext>();
builder.Services.AddSingleton<TicketValidator>();
builder.Services.AddSingleton<TicketsService>();
builder.Services.AddSingleton<TicketTypeService>();
    
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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