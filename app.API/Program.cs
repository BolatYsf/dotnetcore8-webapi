using App.Repositories.Extensions;
using App.Services.Extensions;
using Microsoft.AspNetCore.Mvc;
using App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
  
    opt.Filters.Add<FluentValidationFilter>();
    opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    

});

// i blocked dotnet validation filter
builder.Services.Configure<ApiBehaviorOptions>(opt =>opt.SuppressModelStateInvalidFilter=true);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// inject at repo extensions class .. DI container
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
