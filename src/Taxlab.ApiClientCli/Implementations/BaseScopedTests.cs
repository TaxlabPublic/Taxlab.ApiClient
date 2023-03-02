using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Taxlab.ApiClientCli.Implementations
{
    public abstract class BaseScopedTests : IAsyncLifetime
    {
        public BaseScopedTests(ITestOutputHelper output)
        {
            this.Output = output;
            this.Output.WriteLine("Creating Test Class: '" + this.GetType().Name + "'.");
        }

        protected ITestOutputHelper Output { get; }

        public async Task InitializeAsync()
        {
            await this.SetupTest();
        }

        public async Task DisposeAsync() => await this.TeardownTest();

        protected virtual Task SetupTest() => Task.CompletedTask;

        protected virtual Task TeardownTest() => Task.CompletedTask;

    }
}
