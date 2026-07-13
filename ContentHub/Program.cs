using ContentHub.Api.Extensions;
using ContentHub.Infrastructure.Identity;
using ContentHub.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//-----------------------------------------

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddPersistenceRepositories();
builder.Services.AddApplicationServices();
//-----------------------------------------

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddInfrastructure(builder.Configuration);
//-----------------------------------------

builder.Services.AddScoped<IdentityDataSeeder>();
//-----------------------------------------


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//-----------------------------------------
app.UseGlobalExceptionHandling();
//-----------------------------------------
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//-----------------------------------------
using(var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentityDataSeeder>();

    await seeder.SeedAsync();
}
//-----------------------------------------

app.Run();
