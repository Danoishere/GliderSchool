using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GliderSchool.DataClient
{
    public  static class ServiceConfig
    {
        public static string BaseAddress { get; set; }
    }

    

    public class DataClientBase<TEntityType>
    {
        public string ServiceUrl { get; set; }
        public DataClientBase(string serviceUrl)
        {
            ServiceUrl = serviceUrl;
        }

        public async Task<IList<TEntityType>> GetAll()
        {
            using (var client = CreateHttpClient())
            {
                var response = await client.GetAsync(ServiceUrl);
                if(response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<IList<TEntityType>>(responseString);
                    return result;
                }
                return default(IList<TEntityType>);
            }
        }

        protected HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ServiceConfig.BaseAddress);
            client.Timeout = new TimeSpan(0, 0, 20);
            return client;
        }

    }
}
