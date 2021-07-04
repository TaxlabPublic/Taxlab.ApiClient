using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class EmploymentIncomeRepository : RepositoryBase
    {
        public EmploymentIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfEmploymentIncomeStatementWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string employerName ="",
            string governmentIdentifier ="",
            decimal salaryAndWagesIncome = 0m,
            decimal salaryAndWagesTaxWithheld = 0m,
            bool isOnWorkingHolidayVisa = false,
            decimal allowanceIncome = 0m,
            decimal allowanceTaxWithheld = 0m,
            decimal communityDevelopmentProjectPayments = 0m,
            decimal reportableFringeBenefits = 0m,
            bool isEmployerExemptFromFbt = false,
            decimal superannuationContributions = 0m,
            string countryCode ="",
            decimal exemptForeignGrossIncome = 0m,
            decimal exemptForeignPaymentsInArrears = 0m,
            decimal exemptForeignIncomeTaxPaid = 0m,
            decimal paymentAIncome = 0m,
            string paymentAType ="",
            decimal paymentATaxWithheld = 0m,
            decimal paymentBIncome = 0m,
            decimal paymentBIncomeAssessable = 0m,
            decimal paymentBTaxWithheld = 0m,
            decimal paymentDIncome = 0m,
            decimal paymentEIncome = 0m,
            decimal paymentETaxWithheld = 0m,
            string paymentInArrearsType =""
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetEmploymentIncomeStatementWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.EmployerName = employerName;
            workpaper.GovernmentIdentifier = governmentIdentifier;
            workpaper.SalaryAndWagesIncome = salaryAndWagesIncome.ToNumericCell();
            workpaper.SalaryAndWagesTaxWithheld = salaryAndWagesTaxWithheld.ToNumericCell();
            workpaper.IsOnWorkingHolidayVisa = isOnWorkingHolidayVisa.ToTriState();
            workpaper.AllowanceIncome = allowanceIncome.ToNumericCell();
            workpaper.AllowanceTaxWithheld = allowanceTaxWithheld.ToNumericCell();
            workpaper.CommunityDevelopmentProjectPayments = communityDevelopmentProjectPayments.ToNumericCell();
            workpaper.ReportableFringeBenefits = reportableFringeBenefits.ToNumericCell();
            workpaper.IsEmployerExemptFromFbt = isEmployerExemptFromFbt.ToTriState();
            workpaper.SuperannuationContributions = superannuationContributions.ToNumericCell();
            workpaper.CountryCode = countryCode;
            workpaper.ExemptForeignGrossIncome = exemptForeignGrossIncome.ToNumericCell();
            workpaper.ExemptForeignPaymentsInArrears = exemptForeignPaymentsInArrears.ToNumericCell();
            workpaper.ExemptForeignIncomeTaxPaid = exemptForeignIncomeTaxPaid.ToNumericCell();
            workpaper.PaymentAIncome = paymentAIncome.ToNumericCell();
            workpaper.PaymentAType = paymentAType;
            workpaper.PaymentATaxWithheld = paymentATaxWithheld.ToNumericCell();
            workpaper.PaymentBIncome = paymentBIncome.ToNumericCell();
            workpaper.PaymentBIncomeAssessable = paymentBIncomeAssessable.ToNumericCell();
            workpaper.PaymentBTaxWithheld = paymentBTaxWithheld.ToNumericCell();
            workpaper.PaymentDIncome = paymentDIncome.ToNumericCell();
            workpaper.PaymentEIncome = paymentEIncome.ToNumericCell();
            workpaper.PaymentETaxWithheld = paymentETaxWithheld.ToNumericCell();
            workpaper.PaymentInArrearsType = paymentInArrearsType;

            var command = new UpsertEmploymentIncomeStatementWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostEmploymentIncomeStatementWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
