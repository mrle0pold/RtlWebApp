using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IStorage
    {
        bool ContainsShows();
        QueryResult GetShows(Query query);

        void StoreShows(List<Show> shows);
    }
}
