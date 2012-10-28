using Newtonsoft.Json;

namespace EbookZone.ViewModels
{
    public class FacebookViewModel
    {
        [JsonProperty("id")]
        public string FacebookId { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}