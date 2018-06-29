using Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Persistence
{
    public class Storage : IStorage
    {
        public bool ContainsShows()
        {
            return File.Exists(@"c:\shows.json");
        }

        public QueryResult GetShows(Query query)
        {
            var result = new QueryResult();
            var pageSize = 10;

            var json = File.ReadAllText(@"c:\shows.json");
            var shows = JsonConvert.DeserializeObject<List<Show>>(json);

            result.Shows = shows.Skip(query.Page * pageSize).Take(pageSize).ToArray();
            result.TotalPages = shows.Count / pageSize;
            
            return result;
        }

        public void StoreShows(List<Show> shows)
        {
            File.WriteAllText(@"c:\shows.json", JsonConvert.SerializeObject(shows));
        }
    }
}
