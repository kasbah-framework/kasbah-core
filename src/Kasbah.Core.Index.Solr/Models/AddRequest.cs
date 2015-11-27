using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class Add
    {
        #region Public Properties

        [JsonProperty("boost")]
        public double Boost { get; set; } = 1.0;

        [JsonProperty("commitWithin")]
        public int CommitWithin { get; set; } = 1000;

        [JsonProperty("doc")]
        public IDictionary<string, object> Document { get; set; }

        [JsonProperty("overwrite")]
        public bool Overwrite { get; set; } = true;

        #endregion
    }

    public class AddRequest : BaseRequest
    {
        #region Public Properties

        [JsonProperty("add")]
        public Add Add { get; set; }

        #endregion
    }

    public class AddRequestWithCommit : AddRequest
    {
        #region Public Properties

        [JsonProperty("commit")]
        public object Commit { get; set; } = new object();

        #endregion
    }
}