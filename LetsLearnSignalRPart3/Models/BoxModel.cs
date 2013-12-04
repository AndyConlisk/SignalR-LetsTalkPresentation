using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace LetsLearnSignalRPart3.Models
{
    public class BoxModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("left")]
        public double Left { get; set; }

        [JsonProperty("top")]
        public double Top { get; set; }
    }
}