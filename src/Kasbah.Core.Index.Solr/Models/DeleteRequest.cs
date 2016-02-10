using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class Delete
    {
        #region Public Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        #endregion
    }

    public class DeleteRequest : BaseRequest
    {
        #region Public Properties

        [JsonProperty("delete")]
        public Delete Delete { get; set; }

        #endregion
    }

    public class DeleteRequestWithCommit : DeleteRequest
    {
        #region Public Properties

        [JsonProperty("commit", Order = int.MaxValue)]
        public object Commit { get; set; } = new object();

        #endregion
    }
}