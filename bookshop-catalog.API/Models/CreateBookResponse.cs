using Newtonsoft.Json;

namespace bookshop_catalog.Models
{
    public class CreateBookResponse
    {
        [JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; }
    }
}
