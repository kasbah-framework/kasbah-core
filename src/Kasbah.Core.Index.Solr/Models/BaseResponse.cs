using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class BaseResponse
    {
        #region Public Properties

        [JsonProperty("responseHeader")]
        public ResponseStatus Status { get; set; }

        #endregion
    }
}