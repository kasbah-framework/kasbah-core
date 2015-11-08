using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr
{
    public class SelectResponse : BaseResponse
    {
        [JsonProperty("response")]
        public SelectResponseBody Response { get; set; }
    }

    public class SelectResponseBody
    {
        [JsonProperty("numFound")]
        public int TotalCount { get; set; }

        [JsonProperty("start")]
        public int Offset { get; set; }

        [JsonProperty("docs")]
        public IEnumerable<IDictionary<string, object>> Documents { get; set; }
    }
}