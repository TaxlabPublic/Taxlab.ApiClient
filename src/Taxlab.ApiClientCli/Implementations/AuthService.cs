using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJpbHlhQHRheGxhYi5jby5ueiIsImVtYWlsIjoiaWx5YUB0YXhsYWIuY28ubnoiLCJVc2VybmFtZSI6ImlseWFAdGF4bGFiLmNvLm56IiwiVGF4cGF5ZXJJZCI6IjM5OTg1NTM1LWVjNGQtNGYxNC04YTBkLTI3ZDliNWE0M2Q0MCIsIlZlcnNpb24iOiIiLCJUYXhZZWFyIjoiMjAyMCIsIlVzZXJJZCI6Ijc0ODEiLCJuYmYiOjE2MjQ1MDczMzEsImV4cCI6MTYyNDUxMDkzMSwiaWF0IjoxNjI0NTA3MzMxfQ.I2OGLnH9-F51XD2W9pdDoJt4YxvXH-LVNrDsueEJ7CI";
            return result;
        }
    }
}