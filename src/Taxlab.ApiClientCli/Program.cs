using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli
{
    internal class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        //private const string BaseUrl = "https://localhost:44354/";
        private const string BaseUrl = "https://preview.taxlab.online/api-internal/";
        private static TaxlabApiClient Client;

        private const int TaxYear = 2021; // Change this to your taxYear

        private static async Task Main(string[] args)
        {
  
            var authService = new AuthService();
            Client = new TaxlabApiClient(BaseUrl, HttpClient, authService);


            Console.WriteLine("== Step: Getting existing taxpayers ==========================================================");
            var getAllTaxpayers = await Client.Taxpayers_GetTaxpayersAsync().ConfigureAwait(false);
            var taxpayerService = new TaxpayerRepository(Client);
            var taxpayers = taxpayerService.SearchByTfn("123456789");
            var allTaxpayers = taxpayerService.GetAllTaxpayers();

            Console.WriteLine("== Step: Create deceased taxpayer ==========================================================");
            var deceasedTaxpayerPersonaFactory = new Personas.DeceasedEmployeePersona();
            var deceased = await deceasedTaxpayerPersonaFactory.CreateAsync(Client).ConfigureAwait(false);


            Console.WriteLine("== Step: Create taxpayer with dividend ==========================================================");
            var investorPersonaFactory = new Personas.SingleDividend();
            var investor = await investorPersonaFactory.CreateAsync(Client).ConfigureAwait(false);

            Console.WriteLine("== Step: Create taxpayer with lots of workpapers ==========================================================");
            var complexPersonaFactory = new Personas.ComplexEmployee();
            var complex = await complexPersonaFactory.CreateAsync(Client).ConfigureAwait(false);


            Console.WriteLine("== Step: Get All adjustment workpapers for complex taxpayer ==========================================================");
            // Gets a list of all adjustment workpapers for this taxpayer. 
            // This does not create any new workpapers.
            var allAdjustmentWorkpapers = await Client.Workpapers_AdjustmentWorkpapersAsync(complex.Id, TaxYear, null)
                .ConfigureAwait(false);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("== Step: Get All Taxyear workpapers for complex taxpayer ==========================================================");

            // Gets a list of all Taxyear workpapers for this taxpayer. 
            // This does not create any new workpapers.
            var allATaxYearWorkpapers = await Client.Workpapers_TaxYearWorkpapersAsync(complex.Id, TaxYear, null)
                .ConfigureAwait(false);

        }
    }
}