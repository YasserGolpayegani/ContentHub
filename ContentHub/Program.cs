using ContentHub.Api.Extensions;
using ContentHub.Api.HealthChecks;
using ContentHub.Infrastructure.Identity;
using ContentHub.Infrastructure.Options;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
//using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerService();

//-----------------------------------------

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddPersistenceRepositories();
builder.Services.AddApplicationServices();
//-----------------------------------------

builder.Services.AddInfrastructure(builder.Configuration);
//-----------------------------------------

builder.Services.AddScoped<IdentityDataSeeder>();
//-----------------------------------------

builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddAPI(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ContentHub API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "ContentHub API v2");
    });
}

app.UseHttpsRedirection();
//-----------------------------------------
app.UseCorrelationId();
//-----------------------------------------
app.UseGlobalExceptionHandling();
//-----------------------------------------
app.UseAuthentication();
//-----------------------------------------
app.UseLoggingContext();
app.UseSerilogRequestLogging();
//-----------------------------------------
app.UseAuthorization();


app.MapControllers();

//-----------------------------------------
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentityDataSeeder>();

    await seeder.SeedAsync();
}

//-----------------------------------------
app.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = HealthCheckResponseWriter.WriteResponseAsync });
//-----------------------------------------

app.Run();
