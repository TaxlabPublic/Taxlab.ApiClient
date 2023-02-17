using System;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.Taxpayer
{
    public class TaxPaymentRepository : RepositoryBase
    {
        public TaxPaymentRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<TaxPaymentResponse> CreateAsync(
            Guid taxpayerId,
            int taxYear = 0,
            string description = "description",
            int transactionTypeId = 0,
            bool applyToTaxableIncome = true,
            LocalDate Date = new LocalDate(),
            decimal amount = 0
        )
        {
            var newTaxReturnCommand = new UpsertTaxPaymentCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                Description = description,
                TransactionTypeId = transactionTypeId,
                ApplyToTaxableIncome = applyToTaxableIncome,
                Date = Date.ToAtoDateString(),
                Amount = amount
            };

            var taxReturnResponse = await Client.Taxpayers_PutTaxPaymentsAsync(newTaxReturnCommand).ConfigureAwait(false);

            return taxReturnResponse;
        }
    }
}