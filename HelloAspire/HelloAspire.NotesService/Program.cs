using Microsoft.EntityFrameworkCore;
using HelloAspire.NotesService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults(); // <-- kommt aus dem Aspire-Projekt

builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("notes")));
builder.EnrichNpgsqlDbContext<NotesDbContext>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Apply EF migrations
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    dbContext.Database.Migrate();

    await SeedTestData.SeedData(dbContext);
}

app.UseHttpsRedirection();

app.MapPost("notes", CreateNoteEndpoint.CreateNote);
app.MapGet("notes", GetNotesEndpoint.GetAllNotes);

app.Run();
