using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace bookshop_catalog.Models
{
    public class Book
    {
        /// <summary>
        /// Unique ID for the Book Item
        /// </summary>
        [JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long ID { get; set; }

        /// <summary>
        /// Title of the book
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required(AllowEmptyStrings = false)]
        public string? Title { get; set; }

        /// <summary>
        /// Author of the book
        /// </summary>
        [JsonProperty("author", Required = Required.Always)]
        [Required(AllowEmptyStrings = false)]
        public string? Author { get; set; }

        /// <summary>
        /// Retail price of the book for a single item
        /// </summary>
        [JsonProperty("price", Required = Required.Always)]
        public double Price { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Sortby
    {
        [EnumMember(Value = @"title")]
        Title = 0,

        [EnumMember(Value = @"author")]
        Author = 1,

        [EnumMember(Value = @"price")]
        Price = 2,

    }
}
