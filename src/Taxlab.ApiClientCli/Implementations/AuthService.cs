using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "Enter your token here";
            return result;
        }
    }
}