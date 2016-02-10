using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class CommitRequest : BaseRequest
    {
        #region Public Properties

        [JsonProperty("commit", Order = int.MaxValue)]
        public object Commit { get; set; } = new object();

        #endregion
    }
}