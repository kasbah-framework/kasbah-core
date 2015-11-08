using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class CommitRequest : BaseRequest
    {
        [JsonProperty("commit")]
        public object Commit { get; set; } = new object();
    }
}