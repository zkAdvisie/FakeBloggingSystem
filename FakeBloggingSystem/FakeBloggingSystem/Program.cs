using FakeBloggingSystem.Data;
using FakeBloggingSystem.Interfaces;
using FakeBloggingSystem.Models;
using FakeBloggingSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        })
    .AddXmlSerializerFormatters();


builder.Services.AddDbContext<BlogDbContext>
    (options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BlogDatabase")));

builder.Services.AddScoped<IPostService, PostService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
    app.MapOpenApi();
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    if(!dbContext.Authors.Any())
        {
        dbContext.Authors.Add(new Author { Name = "John", Surname = "Doe" });
        dbContext.Authors.Add(new Author { Name = "Jane", Surname = "Smith" });
        dbContext.SaveChanges();
        }
    }

if(!app.Environment.IsEnvironment("Testing"))
    {
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
    dbContext.Database.Migrate();
    if (!dbContext.Authors.Any())
        {
        dbContext.Authors.Add(new Author { Name = "Test", Surname = "Author" });
        dbContext.SaveChanges();
        }
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program { }