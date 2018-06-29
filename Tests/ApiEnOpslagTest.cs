using Domain;
using ExternalApis;
using Persistence;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ApiEnOpslagTest
    {
        [Fact]
        public async Task IntegratieTest()
        {
            try
            {
                var client = new HttpClient();
                var storage = new Storage();
                var api = new TvMazeApi(client, storage);
                await api.StoreShows();

                var query = new Query()
                {
                    Page = 1
                };
                var shows = storage.GetShows(query);
                Assert.NotNull(shows);
                Assert.True(shows.Shows.Length == 10);

            }
            catch (Exception ex)
            {
                Assert.Null(ex);
            }

        }
    }
}
