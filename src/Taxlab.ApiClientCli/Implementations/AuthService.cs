using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJpbHlhQHRheGxhYi5jby5ueiIsImVtYWlsIjoiaWx5YUB0YXhsYWIuY28ubnoiLCJVc2VybmFtZSI6ImlseWFAdGF4bGFiLmNvLm56IiwiVGF4cGF5ZXJJZCI6ImQ0MGNmZWEyLTQzZjItNGNiZC1hNzY2LTYxMGU3MGQ2YzU4ZCIsIlZlcnNpb24iOiIiLCJUYXhZZWFyIjoiMjAyMSIsIlVzZXJJZCI6Ijc0ODEiLCJuYmYiOjE2MjQ1MDM4MTgsImV4cCI6MTYyNDUwNzQxOCwiaWF0IjoxNjI0NTAzODE4fQ.rC2-h7CXgJdEacNYLuDZtvk49Ddi4cDL_LNej2FMYgo";
            return result;
        }
    }
}