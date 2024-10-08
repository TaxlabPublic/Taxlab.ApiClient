using System.Threading.Tasks;
using System;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.Shared
{
    public class RepositoryBase
    {

        protected readonly TaxlabApiClient Client;
        public RepositoryBase(TaxlabApiClient client)
        {
            Client = client;
        }

        protected async Task<TWorkpaperResponse> GetTaxYearWorkpaperAsync<TWorkpaperResponse>(
            Guid taxpayerId, 
            int taxYear,
            Func<Guid, int, Task<TWorkpaperResponse>> getFunc)
            where TWorkpaperResponse : IMultiTaxYearWorkpaperResponse
        {
            var workpaperResponse = await getFunc(taxpayerId, taxYear)
                .ConfigureAwait(false);

            return workpaperResponse;
        }
        
        protected TUpsertWorkpaperCommand BuildUpsertCommand<TWorkpaperResponse, TUpsertWorkpaperCommand>(
            Guid taxpayerId,
            int taxYear,
            TWorkpaperResponse workpaperResponse,
            WorkpaperType workpaperType)
            where TWorkpaperResponse : IMultiTaxYearWorkpaperResponse
            where TUpsertWorkpaperCommand : BaseTaxYearWorkpaperCommand, new()
        {
            var upsertCommand = new TUpsertWorkpaperCommand
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true
            };

            return upsertCommand;
        }

        protected async Task<TWorkpaperResponse> ExecuteCommandAsync<TWorkpaperResponse, TUpsertWorkpaperCommand>(
            TUpsertWorkpaperCommand upsertCommand,
            Func<TUpsertWorkpaperCommand, Task<TWorkpaperResponse>> upsertFunc)
            where TWorkpaperResponse : IMultiTaxYearWorkpaperResponse
            where TUpsertWorkpaperCommand : BaseTaxYearWorkpaperCommand, new()
        {

            var upsertWorkpaperResponse = await upsertFunc(upsertCommand)
                .ConfigureAwait(false);

            return upsertWorkpaperResponse;
        }
    }
}
