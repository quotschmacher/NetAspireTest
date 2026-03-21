using HelloAspire.NotesService.Data;

internal static class SeedTestData
{
    public static async Task SeedData(NotesDbContext dbContext)
    {
        if (!dbContext.Notes.Any())
        {
            await SeedNotes(dbContext);
        }
    }

    private static async Task SeedNotes(NotesDbContext dbContext)
    {
        var note = new Note
        {
            Id = Guid.CreateVersion7(),
            Title = "TestNote1",
            Content = "Content of Note",
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.Notes.Add(note);
        
        var note2 = new Note
        {
            Id = Guid.CreateVersion7(),
            Title = "TestNote2",
            Content = "Content of Note 2",
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.Notes.Add(note2);
        await dbContext.SaveChangesAsync();
    }
}