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
    }
}
