using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler("/error");

app.MapControllers();

app.Run();
