using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class CommitRequest : BaseRequest
    {
        #region Public Properties

        [JsonProperty("commit")]
        public object Commit { get; set; } = new object();

        #endregion
    }
}