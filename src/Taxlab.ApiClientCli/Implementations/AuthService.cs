using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly string _token;

        public AuthService(string token)
        {
            _token = token;
        }

        public string GetBearerToken()
        {
            return _token;
        }
    }
}