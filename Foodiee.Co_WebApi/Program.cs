using Foodiee.Co_WebApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200") 
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddControllers();

// fetch the connection string from the appSettimgs.json file
var connectionString = builder.Configuration.GetConnectionString("Foodiee.Co_DbConnection");

//DbInjection
// configure the DbContext options for TrainingDbContext
builder.Services.AddDbContext<FoodieeDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FoodieeDbContext>();
    dbContext.Database.Migrate();      // creates tables if not exist
    DbSeeder.Seed(dbContext);          // loads JSON data
}


app.Run();
