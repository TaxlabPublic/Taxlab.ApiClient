using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJpbHlhQHRheGxhYi5jby5ueiIsImVtYWlsIjoiaWx5YUB0YXhsYWIuY28ubnoiLCJVc2VybmFtZSI6ImlseWFAdGF4bGFiLmNvLm56IiwiVGF4cGF5ZXJJZCI6IjRkMGNiZDEwLTBmZDktNDVhMy1hNGQ1LWM4NWIyZTZkM2U5YyIsIlZlcnNpb24iOiIiLCJUYXhZZWFyIjoiMjAyMCIsIlVzZXJJZCI6Ijc0ODEiLCJuYmYiOjE2MjE5MTQ4MTksImV4cCI6MTYyMTkxODQxOSwiaWF0IjoxNjIxOTE0ODE5fQ.7kDVp2YOPQe5nK6Dsh-8cGOThcchuUt8p0AzNwU3O9k";
            return result;
        }
    }
}