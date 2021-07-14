using Newtonsoft.Json;

namespace WebShop.API.Models
{
    public class ApiError
    {
        [JsonProperty("status")] 
        public int Status { get; set; }

        [JsonProperty("title")] 
        public string Title { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

    }

}

