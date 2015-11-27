using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class SelectResponse : BaseResponse
    {
        #region Public Properties

        [JsonProperty("response")]
        public SelectResponseBody Response { get; set; }

        #endregion
    }

    public class SelectResponseBody
    {
        #region Public Properties

        [JsonProperty("docs")]
        public IEnumerable<IDictionary<string, object>> Documents { get; set; }

        [JsonProperty("start")]
        public int Offset { get; set; }

        [JsonProperty("numFound")]
        public int TotalCount { get; set; }

        #endregion
    }
}