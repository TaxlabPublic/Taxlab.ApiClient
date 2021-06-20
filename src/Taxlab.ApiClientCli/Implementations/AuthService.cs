using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJpbHlhQHRheGxhYi5jby5ueiIsImVtYWlsIjoiaWx5YUB0YXhsYWIuY28ubnoiLCJVc2VybmFtZSI6ImlseWFAdGF4bGFiLmNvLm56IiwiVGF4cGF5ZXJJZCI6ImZkMGM0ZDkzLWZhYjgtNDI0NC05ZTVkLTMzM2FiNWU2ODAwNiIsIlZlcnNpb24iOiIiLCJUYXhZZWFyIjoiMjAyMCIsIlVzZXJJZCI6Ijc0ODEiLCJuYmYiOjE2MjM5ODg4MDUsImV4cCI6MTYyMzk5MjQwNSwiaWF0IjoxNjIzOTg4ODA1fQ.1g6DMRr3dUahMtcq1zppIvQwAAhA42P00s5teTJWf1w";
            return result;
        }
    }
}