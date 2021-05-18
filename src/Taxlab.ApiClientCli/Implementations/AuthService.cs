using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJkYXJyZWxAdGF4bGFiLm9ubGluZSIsImVtYWlsIjoiZGFycmVsQHRheGxhYi5vbmxpbmUiLCJVc2VybmFtZSI6ImRhcnJlbEB0YXhsYWIub25saW5lIiwiVGF4cGF5ZXJJZCI6IjBlOWRkMmQ1LWQzYTItNGU5My1hZTM4LWJjM2QzNjJmZDc0NCIsIlZlcnNpb24iOiIiLCJUYXhZZWFyIjoiMjAyMSIsIlVzZXJJZCI6IjgxNzEiLCJuYmYiOjE2MTg4MDM3NzQsImV4cCI6MTYxODgwNzM3NCwiaWF0IjoxNjE4ODAzNzc0fQ.3E1PQkMvqzAwkEAurfgXP1P1aTBVLZ0mUzD_-3Bsam8";
            return result;
        }
    }
}