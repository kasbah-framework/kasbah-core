using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr
{
    public class BaseResponse
    {
        [JsonProperty("responseHeader")]
        public ResponseStatus Status { get; set; }
    }
}