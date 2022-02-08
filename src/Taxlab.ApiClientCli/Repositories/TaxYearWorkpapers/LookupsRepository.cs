using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class LookupsRepository : RepositoryBase
    {
        public LookupsRepository(TaxlabApiClient client) : base(client)
        {

        }
        
        public async Task<IncomeTaxLookups> GetAllLookups()
        {
            return await Client.IncomeTaxContext_LookupsAsync();
        }
    }
}
