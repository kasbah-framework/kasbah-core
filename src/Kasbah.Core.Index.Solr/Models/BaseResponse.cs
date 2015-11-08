using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class BaseResponse
    {
        [JsonProperty("responseHeader")]
        public ResponseStatus Status { get; set; }
    }
}