using System;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers
{
    public class DeclarationsRepository : RepositoryBase
    {
        public DeclarationsRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfDeclarationsWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string taxAgencyName = "",
            string taxAgentNumber = "",
            string taxAgentAbn = "",
            string taxAgentDeclarationStatement = "",
            bool taxAgentDeclarationStatementAccepted = false,
            string taxAgentSignatureFirstName = "",
            string taxAgentSignatureLastName = "",
            string taxAgentSignatureFullName = "",
            string taxAgentContactFirstName = "",
            string taxAgentContactLastName = "",
            string taxAgentContactFullName = "",
            PhoneType taxAgentContactPhoneType = PhoneType.Landline,
            string taxAgentContactPhoneAreaCode = "",
            string taxAgentContactPhoneNumber = "",
            string taxAgentClientReference = "",
            string taxPayerDeclarationStatement = "",
            bool taxPayerDeclarationStatementAccepted = false,
            string taxPayerSignatureFirstName = "",
            string taxPayerSignatureLastName = "",
            string taxPayerSignatureFullName = ""
        )
        {
            var workpaperResponse = await Client
                .Workpapers_GetDeclarationsWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.TaxpayerDetailsWorkpaper, Guid.Empty, false, false)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxAgencyName = taxAgencyName;
            workpaper.TaxAgentNumber = taxAgentNumber;
            workpaper.TaxAgentABN = taxAgentAbn;
            workpaper.TaxAgentDeclarationStatement = taxAgentDeclarationStatement;
            workpaper.TaxAgentDeclarationStatementAccepted = taxAgentDeclarationStatementAccepted;
            workpaper.TaxAgentSignatureFirstName = taxAgentSignatureFirstName;
            workpaper.TaxAgentSignatureLastName = taxAgentSignatureLastName;
            workpaper.TaxAgentSignatureFullName = taxAgentSignatureFullName;
            workpaper.TaxAgentContactFirstName = taxAgentContactFirstName;
            workpaper.TaxAgentContactLastName = taxAgentContactLastName;
            workpaper.TaxAgentContactFullName = taxAgentContactFullName;
            workpaper.TaxAgentContactPhoneType = taxAgentContactPhoneType;
            workpaper.TaxAgentContactPhoneAreaCode = taxAgentContactPhoneAreaCode;
            workpaper.TaxAgentContactPhoneNumber = taxAgentContactPhoneNumber;
            workpaper.TaxAgentClientReference = taxAgentClientReference;
            workpaper.TaxPayerDeclarationStatement = taxPayerDeclarationStatement;
            workpaper.TaxPayerDeclarationStatementAccepted = taxPayerDeclarationStatementAccepted;
            workpaper.TaxPayerSignatureFirstName = taxPayerSignatureFirstName;
            workpaper.TaxPayerSignatureLastName = taxPayerSignatureLastName;
            workpaper.TaxPayerSignatureFullName = taxPayerSignatureFullName;

            // Update command for our new workpaper
            var upsertCommand = new UpsertDeclarationsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.DeclarationsWorkpaper,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostDeclarationsWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }
    }
}
