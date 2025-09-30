using Vkr.Domain.Enums.Files;
using File =  Vkr.Domain.Entities.Files.File;

namespace Vkr.Application.Interfaces.Repositories.FilesRepositories
{
    public interface IFilesRepository
    {
        Task<File>                   AddAsync(File file);
        Task<File?>                  GetAsync(int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<File>>      ListByOwnerAsync(FileOwnerType ownerType, int ownerId);
    }
}
