using Consumer.General;
using Filters;
using Newtonsoft.Json;
using Resources;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Consumer.Services
{
    public interface IAuthorService
    {
        public Task<List<AuthorResource>> GetAll();

        public Task<AuthorResource> GetById(long id);
    }

    public class AuthorService : IAuthorService
    {
        private readonly string _endPoint;

        private readonly HttpClient _httpClient;

        public AuthorService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _endPoint = Configuration.SourceEndPoint + "Author/";
        }

        public async Task<List<AuthorResource>> GetAll()
        {
            Uri Uri = new Uri(_endPoint);

            HttpResponseMessage response = await _httpClient.GetAsync(Uri);

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AuthorResource>>(content);

        }

        public async Task<AuthorResource> GetById(long id)
        {
            HttpResponseMessage Response = await _httpClient.GetAsync(new Uri(_endPoint + id.ToString()));

            string Content = await Response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AuthorResource>(Content);
        }
    }
}
