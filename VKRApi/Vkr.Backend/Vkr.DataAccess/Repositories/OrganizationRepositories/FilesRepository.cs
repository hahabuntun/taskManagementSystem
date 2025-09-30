using Microsoft.EntityFrameworkCore;
using Vkr.Application.Interfaces.Repositories.FilesRepositories;
using Vkr.Domain.Enums.Files;
using File = Vkr.Domain.Entities.Files.File;

namespace Vkr.DataAccess.Repositories.OrganizationRepositories;

public class FilesRepository(ApplicationDbContext ctx) : IFilesRepository
{
    private readonly DbSet<File> _db = ctx.Set<File>();

    public async Task<File> AddAsync(File file)
    {
        _db.Add(file);
        await ctx.SaveChangesAsync();
        return file;
    }

    public Task<File?> GetAsync(int id)
        => _db.FindAsync(id).AsTask();

    public async Task DeleteAsync(int id)
    {
        var f = await _db.FindAsync(id);
        if (f != null)
        {
            _db.Remove(f);
            await ctx.SaveChangesAsync();
        }
    }

    public Task<IEnumerable<File>> ListByOwnerAsync(FileOwnerType ownerType, int ownerId)
    {
        return _db
            .Where(f => f.OwnerType == ownerType && f.OwnerId == ownerId)
            .Include(f => f.Creator)
            .AsNoTracking()
            .ToListAsync()
            .ContinueWith(t => (IEnumerable<File>)t.Result);
    }
}