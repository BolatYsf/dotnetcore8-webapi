using App.Application.Extensions;
using App.Persistence.Extensions;
using Clean.API.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Controller
//builder.Services.AddControllers(opt =>
//{

//    opt.Filters.Add<FluentValidationFilter>();
//    opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;


//});
#endregion
// extensions 
builder.Services.AddControllersWithFiltersExt();

// i blocked dotnet validation filter
builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressModelStateInvalidFilter = true);

#region Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
#endregion

builder.Services.AddSwaggerGenExt();

// inject at repo extensions class .. DI container
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);

//builder.Services.AddScoped(typeof(NotFoundFilter<,>));

#region Exception
//builder.Services.AddExceptionHandler<CriticalExceptionHandler>();
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
#endregion
builder.Services.AddExceptionHandlerExt();

#region CacheService
//builder.Services.AddMemoryCache();
//builder.Services.AddSingleton<ICacheService,CacheService>();
#endregion
builder.Services.CachingExt();

var app = builder.Build();

app.UseExceptionHandler(x => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    #region Swagger
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    #endregion
    app.UseSwaggerExt();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
