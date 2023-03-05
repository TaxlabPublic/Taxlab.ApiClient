using System;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using Xunit;
using System.Collections.Generic;

namespace Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers
{
    public class ForeignEmploymentIncomeStatementRepository : RepositoryBase
    {
        public ForeignEmploymentIncomeStatementRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfForeignEmploymentIncomeStatementWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string employerName = "",
            string governmentIdentifier = "",
            string foreignIncomeType = "",
            decimal grossIncome = 0m,
            decimal taxWithheld = 0m,
            decimal reportableFringeBenefits = 0m,
            TriState isEmployerExemptFromFbt = TriState.Unset,
            decimal superannuationContributions = 0m,
            decimal paymentAIncome = 0m,
            string paymentAType = "",
            decimal paymentDIncome = 0m,
            decimal paymentEIncome = 0m,
            List<Payments> paymentsInArrears = null,
            decimal paymentsInArrearsTotal = 0m,
            decimal foreignWorkRelatedDeductions = 0m,
            decimal netIncome = 0m,
            decimal taxPaid = 0m,    
            decimal nonRefundableTaxOffset = 0m,
            string residencyStatus = "",
            List<CountryOfResidence> countriesOfResidence = null,
            decimal permanentDifference = 0m)
        {
            var workpaperResponse = await Client
                .Workpapers_GetForeignEmploymentIncomeStatementWorkpaperAsync(
                    taxpayerId,
                    taxYear, Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.EmployerName = employerName;
            workpaper.GovernmentIdentifier = governmentIdentifier;
            workpaper.ForeignIncomeType = foreignIncomeType;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.TaxWithheld = taxWithheld.ToNumericCell();
            workpaper.ReportableFringeBenefits = reportableFringeBenefits.ToNumericCell();
            workpaper.IsEmployerExemptFromFbt = isEmployerExemptFromFbt;
            workpaper.SuperannuationContributions = superannuationContributions.ToNumericCell();
            workpaper.PaymentAIncome = paymentAIncome.ToNumericCell();
            workpaper.PaymentAType = paymentAType;
            workpaper.PaymentDIncome = paymentDIncome.ToNumericCell();
            workpaper.PaymentEIncome = paymentEIncome.ToNumericCell();
            workpaper.PaymentsInArrears = paymentsInArrears;
            workpaper.PaymentsInArrearsTotal = paymentsInArrearsTotal;
            workpaper.ForeignWorkRelatedDeductions = foreignWorkRelatedDeductions.ToNumericCell();
            workpaper.NetIncome = netIncome;
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.NonRefundableTaxOffset = nonRefundableTaxOffset.ToNumericCell();
            workpaper.ResidencyStatus = residencyStatus;
            workpaper.CountriesOfResidence = countriesOfResidence;
            workpaper.PermanentDifference = permanentDifference;


            var command = new UpsertForeignEmploymentIncomeStatementWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostForeignEmploymentIncomeStatementWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
