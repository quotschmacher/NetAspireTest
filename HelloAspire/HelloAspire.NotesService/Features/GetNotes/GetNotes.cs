using Microsoft.AspNetCore.Mvc;
using HelloAspire.NotesService.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

internal static class GetNotesEndpoint
{
    public static async Task<IResult> GetAllNotes(
        NotesDbContext dbContext)
    {
        var retval = await dbContext.Notes.ToListAsync();
        return Results.Ok(retval);
    }
}