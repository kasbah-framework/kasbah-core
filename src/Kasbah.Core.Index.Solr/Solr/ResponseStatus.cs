using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr
{
    public class ResponseStatus
    {
        [JsonProperty("status")]
        public int Code { get; set; }

        [JsonProperty("QTime")]
        public int QueryTime { get; set; }
    }
}