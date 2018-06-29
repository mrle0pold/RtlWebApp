using Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExternalApis
{
    public class TvMazeApi : IExternalApi
    {
        private HttpClient HttpClient { get; set; }
        private IStorage Storage { get; set; }


        public TvMazeApi(HttpClient httpClient, IStorage storage)
        {
            HttpClient = httpClient;
            Storage = storage;
        }
        public async Task StoreShows()
        {
            var shows = await GetShowsAsync();
            shows = InsertCastInShows(shows);
            Storage.StoreShows(shows);
            
        }

        private async Task<List<Show>> GetShowsAsync()
        {

            HttpClient.BaseAddress = new Uri("http://api.tvmaze.com/");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await HttpClient.GetAsync("shows");
            if(response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Show>>(stringResult);
            }
            return new List<Show>();
        }

        private List<Show> InsertCastInShows(List<Show> shows)
        {
            shows.ForEach(async show =>
            {
                System.Threading.Thread.Sleep(200);
                show.Cast = await RetrieveAndMapCast(show);
            });
            return shows;
        }

        private async Task<CastMember[]> RetrieveAndMapCast(Show show, bool throttle = false)
        {
            if (throttle)
            {
                System.Threading.Thread.Sleep(500);
            }
            var response = await HttpClient.GetAsync($"shows/{show.Id}/cast");
            if (response.IsSuccessStatusCode)
            {
                var stringResult = await response.Content.ReadAsStringAsync();

                var jObject = JArray.Parse(stringResult);
                var persons = jObject.Children().Select(item => item["person"]).ToList();
                var result = persons.Select(person => person.ToObject<CastMember>()).ToArray();
                return result.OrderByDescending(cast => cast.BirthDay).ToArray();
            }

            if(response.StatusCode == (HttpStatusCode)429)
            {
                return await RetrieveAndMapCast(show, throttle: true);
            }

            throw new Exception("Geen crew te vinden");
        }
    }
}
