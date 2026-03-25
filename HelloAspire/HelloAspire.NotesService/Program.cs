using Microsoft.EntityFrameworkCore;
using HelloAspire.NotesService.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults(); // <-- kommt aus dem Aspire-Projekt

var test = builder.Configuration.GetConnectionString("notes");

builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("notes")));
builder.EnrichNpgsqlDbContext<NotesDbContext>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer("keycloak", realm: "notes", options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = "account";

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = new[]
            {
                "http://localhost:8080/realms/notes",
                "http://keycloak:8080/realms/notes"
            }
        };
    });

builder.Services.AddControllers();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });

    // Apply EF migrations
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    dbContext.Database.Migrate();

    await SeedTestData.SeedData(dbContext);
}

app.UseHttpsRedirection();

app.MapPost("notes", CreateNoteEndpoint.CreateNote);
app.MapGet("notes", GetNotesEndpoint.GetAllNotes).RequireAuthorization();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
