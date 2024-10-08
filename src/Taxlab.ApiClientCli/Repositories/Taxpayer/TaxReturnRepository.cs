using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.Taxpayer
{
    public class TaxReturnRepository : RepositoryBase
    {
        public TaxReturnRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<TaxReturnResponse> CreateAsync(
            Guid taxpayerId,
            int taxYear = 0,
            DateOnly balanceDate = default,
            DateOnly startDate = default
        )
        {
            var newTaxReturnCommand = new UpsertTaxReturnCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                BalanceDate = balanceDate.ToString("yyyy-MM-dd"),
                StartDate = startDate.ToString("yyyy-MM-dd")
            };

            var taxReturnResponse = await Client.Taxpayers_PutTaxReturnAsync(newTaxReturnCommand).ConfigureAwait(false);

            return taxReturnResponse;
        }
    }
}
