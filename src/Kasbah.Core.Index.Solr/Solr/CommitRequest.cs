using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr
{
    public class CommitRequest : BaseRequest
    {
        [JsonProperty("commit")]
        public object Commit { get; set; } = new object();
    }
}