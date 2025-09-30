using Vkr.Domain.Entities;
using Vkr.Domain.Entities.Worker;

namespace Vkr.Application.Interfaces.Infractructure
{
    public interface IJwtProvider
    {
        string GenerateToken(Workers user);
    }
}
