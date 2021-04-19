using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "<< Your Bearer Token Goes Here >>";
            return result;
        }
    }
}