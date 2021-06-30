using Newtonsoft.Json;
using Resources;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Consumer.Services
{
    public interface IAuthorService
    {
        Task<AuthorResource> GetById(long Id);
    }

    public class AuthorService : IAuthorService
    {
        private readonly HttpClient _httpClient;

        private readonly string _endPoint;

        public AuthorService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._endPoint = Configuration.SourceEndPoint + "Author/";
        }

        public async Task<AuthorResource> GetById(long Id)
        {
            HttpResponseMessage response = await this._httpClient.GetAsync(new Uri(this._endPoint + Id.ToString()));
            string content = await response.Content.ReadAsStringAsync();
            AuthorResource authorResource = JsonConvert.DeserializeObject<AuthorResource>(content);
            return authorResource;
        }
    }
}
