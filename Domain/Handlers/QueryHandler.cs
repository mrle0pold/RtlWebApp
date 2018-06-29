using System.Threading.Tasks;

namespace Domain
{
    public class QueryHandler
    {
        public IExternalApi ExternalApi { get; set; }

        public IStorage Storage { get; set; }

        public QueryHandler(IExternalApi externalApi, IStorage storage)
        {
            ExternalApi = externalApi;
            Storage = storage;
        }
        public async Task<QueryResult> Handle(Query query)
        {
            var result = new QueryResult();
            if(!Storage.ContainsShows())
            {
                await ExternalApi.StoreShows();
                
            }
            result = Storage.GetShows(query);
            return result;
        }
    }
}
