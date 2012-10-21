using Newtonsoft.Json;

namespace EbookZone.Web.Models
{
    public class TwitterViewModel
    {
        [JsonProperty("id_str")]
        public string TwitterId { get; set; }

        [JsonProperty("screen_name")]
        public string UserName { get; set; }
    }
}