using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class DeleteRequest : BaseRequest
    {
        [JsonProperty("delete")]
        public Delete Delete { get; set; }
    }

    public class DeleteRequestWithCommit : DeleteRequest
    {
        [JsonProperty("commit")]
        public object Commit { get; set; } = new object();
    }

    public class Delete
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}