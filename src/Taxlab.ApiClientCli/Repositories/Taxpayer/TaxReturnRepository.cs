using System;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
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
            LocalDate balanceDate = new LocalDate(),
            LocalDate startDate = new LocalDate()
        )
        {
            var newTaxReturnCommand = new UpsertTaxReturnCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                BalanceDate = balanceDate.ToAtoDateString(),
                StartDate = startDate.ToAtoDateString()
            };

            var taxReturnResponse = await Client.Taxpayers_PutTaxReturnAsync(newTaxReturnCommand).ConfigureAwait(false);

            return taxReturnResponse;
        }
    }
}
