using System;
using System.Threading.Tasks;
using NodaTime;
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
            LocalDate dateOfBirth,
            LocalDate? dateOfDeath = null,
            bool nameChangedSinceLastReturn = false,
            bool finalReturn = false,
            bool hasAddressChangedSinceLastReturn = false,
            string mobilePhoneNumber ="",
            string daytimeAreaPhoneCode ="",
            string daytimePhoneNumber ="",
            string emailAddress ="",
            string residencyStatus ="",
            LocalDate? residencyStartDate = null,
            LocalDate? residencyEndDate = null,
            string bsbNumber ="",
            string bankAccountNumber ="",
            string bankAccountName ="",
            bool consentFamilyAssistanceDebt = false,
            string spouseCRN ="",
            bool baseRateEntityIndicator = false,
            int? familyTrustElectionYear = null,
            string familyTrustElection = ""

            )
        {
            var taxpayerDetailsWorkpaperResponse = await Client
                .Workpapers_GetTaxpayerDetailsWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.TaxpayerDetailsWorkpaper, Guid.Empty, false, false, true)
                .ConfigureAwait(false);

            var workpaper = taxpayerDetailsWorkpaperResponse.Workpaper;
            workpaper.NameChangedSinceLastReturn = nameChangedSinceLastReturn;
            workpaper.DateOfBirth = dateOfBirth.ToAtoDateString();
            workpaper.DateOfDeath = dateOfDeath.ToAtoDateString();
            workpaper.FinalReturn = finalReturn;
            workpaper.HasAddressChangedSinceLastReturn = hasAddressChangedSinceLastReturn;
            workpaper.MobilePhoneNumber = mobilePhoneNumber;
            workpaper.DaytimePhoneAreaCode = daytimeAreaPhoneCode;
            workpaper.DaytimePhoneNumber = daytimePhoneNumber;
            workpaper.EmailAddress = emailAddress;
            workpaper.ResidencyStatus = residencyStatus;
            workpaper.ResidencyStartDate = residencyStartDate.ToString();
            workpaper.ResidencyEndDate = residencyEndDate.ToString();
            workpaper.BsbNumber = bsbNumber;
            workpaper.BankAccountNumber = bankAccountNumber;
            workpaper.BankAccountName = bankAccountName;
            workpaper.ConsentFamilyAssistanceDebt = consentFamilyAssistanceDebt.ToTriState();
            workpaper.SpouseCRN = spouseCRN;
            workpaper.BaseRateEntityIndicator = baseRateEntityIndicator;
            workpaper.FamilyTrustElectionYear = familyTrustElectionYear;
            workpaper.FamilyTrustElection = familyTrustElection;

            // Update command for our new workpaper
            var upsertTaxpayerDetailsCommand = new UpsertTaxpayerDetailsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = taxpayerDetailsWorkpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.TaxpayerDetailsWorkpaper,
                Workpaper = taxpayerDetailsWorkpaperResponse.Workpaper
            };

            var upsertTaxpayerDetailsResponse = await Client.Workpapers_PostTaxpayerDetailsWorkpaperAsync(upsertTaxpayerDetailsCommand)
                .ConfigureAwait(false);

            return upsertTaxpayerDetailsResponse;
        }
    }
}
