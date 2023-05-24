using Delivering.Application;
using Delivering.Infrastructure;
using Delivering.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();