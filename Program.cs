using BEDAssignment2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using BEDAssignment2.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddDbContext<ModelDB>(opt => opt.UseInMemoryDatabase("ModelList"));

//SignalR 
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo{
        Title = "Model Management",
        Description = "Dataregistration: Models, Jobs & Expenses.",
        Version = "v1"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint(
        "/swagger/v1/swagger.json", "DefaultApiTemplate v1"));
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

// Map SignalR Hub
app.MapHub<ExpenseHub>("/expenseHub");

app.Run();
