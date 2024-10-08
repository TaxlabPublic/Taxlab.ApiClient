using System;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers
{
    public class TaxpayerDetailsRepository : RepositoryBase
    {
        public TaxpayerDetailsRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfTaxpayerDetailsWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            DateOnly dateOfBirth,
            DateOnly? dateOfDeath = null,
            bool nameChangedSinceLastReturn = false,
            bool finalReturn = false,
            bool hasAddressChangedSinceLastReturn = false,
            string mobilePhoneNumber ="",
            string daytimeAreaPhoneCode ="",
            string daytimePhoneNumber ="",
            string emailAddress ="",
            string residencyStatus ="",
            DateOnly? residencyStartDate = null,
            DateOnly? residencyEndDate = null,
            string bsbNumber ="",
            string bankAccountNumber ="",
            string bankAccountName ="",
            bool consentFamilyAssistanceDebt = false,
            string spouseCRN ="",
            bool baseRateEntityIndicator = false,
            int? familyTrustElectionYear = null,
            string familyTrustElection = "",
            decimal higherEducationLoanProgramBalance = 0,
            decimal vetStudentLoanBalance = 0,
            decimal studentFinancialSupplementSchemeBalance = 0,
            decimal studentStartupLoanBalance = 0,
            decimal abstudyStudentStartupLoanBalance = 0,
            decimal tradeSupportLoanBalance = 0, 
            bool smallBusinessIndicator = false
        )
        {
            var taxpayerDetailsWorkpaperResponse = await Client
                .Workpapers_GetTaxpayerDetailsWorkpaperAsync(taxpayerId, taxYear)
                .ConfigureAwait(false);

            var workpaper = taxpayerDetailsWorkpaperResponse.Workpaper;
            workpaper.NameChangedSinceLastReturn = nameChangedSinceLastReturn;
            workpaper.DateOfBirth = dateOfBirth.ToDateTime(default);
            workpaper.DateOfDeath = dateOfDeath?.ToDateTime(default);
            workpaper.FinalReturn = finalReturn;
            workpaper.HasAddressChangedSinceLastReturn = hasAddressChangedSinceLastReturn;
            workpaper.MobilePhoneNumber = mobilePhoneNumber;
            workpaper.DaytimePhoneAreaCode = daytimeAreaPhoneCode;
            workpaper.DaytimePhoneNumber = daytimePhoneNumber;
            workpaper.EmailAddress = emailAddress;
            workpaper.ResidencyStatus = residencyStatus;
            workpaper.ResidencyStartDate = residencyStartDate?.ToDateTime(default);
            workpaper.ResidencyEndDate = residencyEndDate?.ToDateTime(default);
            workpaper.BsbNumber = bsbNumber;
            workpaper.BankAccountNumber = bankAccountNumber;
            workpaper.BankAccountName = bankAccountName;
            workpaper.ConsentFamilyAssistanceDebt = consentFamilyAssistanceDebt.ToTriState();
            workpaper.SpouseCRN = spouseCRN;
            workpaper.BaseRateEntityIndicator = baseRateEntityIndicator;
            workpaper.FamilyTrustElectionYear = familyTrustElectionYear;
            workpaper.FamilyTrustElection = familyTrustElection;
            workpaper.HigherEducationLoanProgramBalance = higherEducationLoanProgramBalance;
            workpaper.VetStudentLoanBalance = vetStudentLoanBalance;
            workpaper.StudentFinancialSupplementSchemeBalance = studentFinancialSupplementSchemeBalance;
            workpaper.StudentStartupLoanBalance = studentStartupLoanBalance;
            workpaper.AbstudyStudentStartupLoanBalance = abstudyStudentStartupLoanBalance;
            workpaper.TradeSupportLoanBalance = tradeSupportLoanBalance;
            workpaper.SmallBusinessIndicator = smallBusinessIndicator;

            // Update command for our new workpaper
            var upsertTaxpayerDetailsCommand = new UpsertTaxpayerDetailsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = taxpayerDetailsWorkpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                Workpaper = taxpayerDetailsWorkpaperResponse.Workpaper
            };

            var upsertTaxpayerDetailsResponse = await Client.Workpapers_UpsertTaxpayerDetailsWorkpaperAsync(upsertTaxpayerDetailsCommand)
                .ConfigureAwait(false);

            return upsertTaxpayerDetailsResponse;
        }
    }
}
