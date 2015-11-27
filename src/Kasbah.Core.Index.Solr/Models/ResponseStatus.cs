using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class ResponseStatus
    {
        #region Public Properties

        [JsonProperty("status")]
        public int Code { get; set; }

        [JsonProperty("QTime")]
        public int QueryTime { get; set; }

        #endregion
    }
}