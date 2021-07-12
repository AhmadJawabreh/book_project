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


    public interface IPublisherService
    {
        public Task<List<PublisherResource>> GetAll();

        public Task<PublisherResource> GetById(long id);
    }

    public class PublisherService : IPublisherService
    {
        private readonly HttpClient _httpClient;

        private readonly string _endPoint;

        public PublisherService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _endPoint = Configuration.SourceEndPoint + "Publisher";
        }

        public async Task<List<PublisherResource>> GetAll()
        {
            Uri uri = new Uri(_endPoint);

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            string content = await response.Content.ReadAsStringAsync();

            List<PublisherResource> publisherResources = JsonConvert.DeserializeObject<List<PublisherResource>>(content);

            return publisherResources;
        }

        public async Task<PublisherResource> GetById(long id)
        {
            Uri uri = new Uri(_endPoint+ "/" + id.ToString());

            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            string content = await response.Content.ReadAsStringAsync();

            PublisherResource PublisherResource = JsonConvert.DeserializeObject<PublisherResource>(content);

            return PublisherResource;
        }
    }
}
