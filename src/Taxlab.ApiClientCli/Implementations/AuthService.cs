﻿using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientCli.Implementations
{
    public class AuthService : IAuthService
    {
        public string GetBearerToken()
        {
            var result = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ilg1ZVhrNHh5b2pORnVtMWtsMll0djhkbE5QNC1jNTdkTzZRR1RWQndhTmsifQ.eyJpc3MiOiJodHRwczovL3RheGxhYnB1YmxpYy5iMmNsb2dpbi5jb20vYjcyYzNmZjgtMzY2MS00NGFiLTlkZjMtZTZiMzg1YjVlNzZmL3YyLjAvIiwiZXhwIjoxNjI3NDMyNjA2LCJuYmYiOjE2Mjc0MjkwMDYsImF1ZCI6IjVlYzQ0MjM1LTY1MTEtNDg1ZS1iZjkxLWViNzVlNDYzZjYyMCIsIm9pZCI6IjZkMjRjYmUzLTA0YjMtNGUzYS1iZWUxLTJhOWEzZWMxYjcwYiIsInN1YiI6IjZkMjRjYmUzLTA0YjMtNGUzYS1iZWUxLTJhOWEzZWMxYjcwYiIsImZhbWlseV9uYW1lIjoiVEVTVCIsImdpdmVuX25hbWUiOiJURVNUIiwibmFtZSI6IlRFU1QiLCJlbWFpbHMiOlsia2FybDVAdGF4bGFiLmVtYWlsIl0sInRmcCI6IkIyQ18xX0N1c3RvbUJhc2UiLCJub25jZSI6IjYzNzYzMDI1Nzg2MTMxNzcwOC5aVEl3T0RsbU5qRXRNRE5tTmkwME1qbGlMV0ptWldVdE5qbGhZalJsTldabE16Wm1NVFpsTm1SalpUWXRPVEV3TXkwME1XUmpMV0kyWkdVdFltUmxOMll3WTJRM1pUZzAiLCJzY3AiOiJhcGkuYWNjZXNzIiwiYXpwIjoiN2Y1ZGFmMTItZjgzMS00NjRkLTk4YzUtMmEzNzZiZTExYzEyIiwidmVyIjoiMS4wIiwiaWF0IjoxNjI3NDI5MDA2fQ.U9a9_sZ5TuQsNHb52J53iqKoMwYIQ4kLQ9YEGeRZn82H1RwXDGon3-70vqUDX10ZdAXSOfb2PvjMJllGeCjENXt2vqKjIkzDz-8d8fOe9HMdBcV1T-ooaWrln5lZTI76pFI-UAo5KryzPUeeTuqImzobQWcTok1cR-wM9GmowWNKHypY5RHGuPiKG_VNPOb31HuoF880bFCQpacsArh0ZADu6PZqHQMBgcVKCxpp5lEi7WiRZARaVWt1Qau8YFpRBUAVbOLVqyGgnssgP-Ul4LJWoSP6FGZkNzr2ygWFxMJ1m53UIVTiahdGcBWK1rFz2VIRZNXCg2Bac6Mri_TeqA";
            return result;
        }
    }
}