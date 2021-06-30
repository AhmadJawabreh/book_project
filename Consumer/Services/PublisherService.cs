using Newtonsoft.Json;
using Resources;
using System.Net.Http;
using System.Threading.Tasks;

namespace Consumer.Services
{
    public interface IPublisherService
    {
        Task<PublisherResource> GetById(long Id);
    }

    internal class PublisherService : IPublisherService
    {
        private readonly HttpClient _httpClient;

        private readonly string _endPoint;

        public PublisherService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            this._endPoint = "http;//localhost:5000/";
        }

        public async Task<PublisherResource> GetById(long Id)
        {
            HttpResponseMessage response = await this._httpClient.GetAsync(this._endPoint + Id.ToString());
            string content = await response.Content.ReadAsStringAsync();
            PublisherResource PublisherResource = JsonConvert.DeserializeObject<PublisherResource>(content);
            return PublisherResource;
        }
    }
}
