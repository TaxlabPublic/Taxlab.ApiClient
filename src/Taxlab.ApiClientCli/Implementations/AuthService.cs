using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJpbHlhQHRheGxhYi5jby5ueiIsImVtYWlsIjoiaWx5YUB0YXhsYWIuY28ubnoiLCJVc2VybmFtZSI6ImlseWFAdGF4bGFiLmNvLm56IiwiVGF4cGF5ZXJJZCI6ImZkMGM0ZDkzLWZhYjgtNDI0NC05ZTVkLTMzM2FiNWU2ODAwNiIsIlZlcnNpb24iOiIiLCJUYXhZZWFyIjoiMjAyMCIsIlVzZXJJZCI6Ijc0ODEiLCJuYmYiOjE2MjQ0MTk0NTcsImV4cCI6MTYyNDQyMzA1NywiaWF0IjoxNjI0NDE5NDU3fQ.jA2JaXvzvbUCq1zKbYY_SRYzu8lRnqYaD43STe0sfQI";
            return result;
        }
    }
}