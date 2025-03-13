using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wasty.Models
{
    class Token
    {
        [JsonInclude]
        public string token { get; set; }
        [JsonInclude]
        public string tokenRefrendacion { get; set; }
    }
}