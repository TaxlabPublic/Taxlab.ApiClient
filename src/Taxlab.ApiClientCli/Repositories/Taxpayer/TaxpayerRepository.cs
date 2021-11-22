using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using TaxLab;

namespace Taxlab.ApiClientCli.Repositories.Taxpayer
{
    public class TaxpayerRepository : RepositoryBase
    {
        private readonly ResourceFileLoader _resourceFileLoader = new ResourceFileLoader(typeof(TaxpayerRepository));

        public TaxpayerRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<TaxpayerResponse> CreateAsync(
            int taxYear,
            string firstName = "",
            string lastName = "",
            string taxFileNumber = "",
            EntityType entityType = EntityType.IndividualAU
        )
        {
            var newtaxpayerCommand = new UpsertTaxpayerCommand
            {
                EntityType = entityType,
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
