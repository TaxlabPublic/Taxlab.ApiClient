using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.Taxpayer
{
    public class TaxpayerRepository : RepositoryBase
    {
        public TaxpayerRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<TaxpayerResponse> CreateAsync(
            int taxYear,
            string firstName,
            string lastName,
            string taxFileNumber
        )
        {
            var newtaxpayerCommand = new UpsertTaxpayerCommand
            {
                EntityType = EntityType.IndividualAU,
                TaxpayerId = Guid.Empty,
                TaxpayerOrFirstName = firstName,
                LastName = lastName,
                TaxFileNumber = taxFileNumber,
                TaxYear = taxYear
            };

            var newTaxpayerResponse = await Client.Taxpayers_PutTaxpayerAsync(newtaxpayerCommand)
                .ConfigureAwait(false);

            return newTaxpayerResponse;
        }


        public async Task<TaxpayerListResponse> SearchByTfn(
            string taxFileNumber
        )
        {
            var result = await Client.Taxpayers_GetSearchTaxpayersAsync(taxFileNumber).ConfigureAwait(false); ;
            return result;
        }
        public async Task<TaxpayerListResponse> GetAllTaxpayers()
        {
            var result = await Client.Taxpayers_GetTaxpayersAsync().ConfigureAwait(false);
            return result;
        }
    }
}
