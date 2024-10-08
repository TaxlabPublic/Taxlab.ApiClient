using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class DistributionsRepository : RepositoryBase
    {
        public DistributionsRepository(TaxlabApiClient client) : base(client)
        {

        }
    }
}