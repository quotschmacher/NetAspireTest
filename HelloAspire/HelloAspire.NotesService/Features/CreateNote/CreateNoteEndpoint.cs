using Microsoft.AspNetCore.Mvc;
using HelloAspire.NotesService.Data;

internal static class CreateNoteEndpoint
{
    public record Request(string Title, string Content);
    public record Response(Guid Id, string Title, string Content, DateTime CreatedAt);

    public static async Task<IResult> CreateNote(
        [FromBody] Request request,
        NotesDbContext dbContext,
        ILogger<Program> logger)
    {
        try
        {
            var note = new Note
            {
                Id = Guid.CreateVersion7(),
                Title = request.Title,
                Content = request.Content,
                CreatedAtUtc = DateTime.UtcNow
            };
    
            dbContext.Notes.Add(note);
            await dbContext.SaveChangesAsync();
    
            var response = new Response(note.Id, note.Title, note.Content, note.CreatedAtUtc);
    
            return Results.Created($"notes/{note.Id}", response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating note");
            return Results.Problem("An error occured");
        }
    }
}