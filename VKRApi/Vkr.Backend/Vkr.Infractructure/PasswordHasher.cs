using Vkr.Application.Interfaces.Infractructure;

namespace Vkr.Infractructure
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password)
        {
            var res = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            return res;
        }

        public bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}
