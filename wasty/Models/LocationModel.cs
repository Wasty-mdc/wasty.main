using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class LocationModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }
        [JsonPropertyName("province")]
        public string Province { get; set; }
        [JsonPropertyName("poblacion")]
        public string Town { get; set; }
    }
}