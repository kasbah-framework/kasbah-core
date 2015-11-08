using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Models
{
    public class Add
    {
        [JsonProperty("doc")]
        public IDictionary<string, object> Document { get; set; }

        [JsonProperty("boost")]
        public double Boost { get; set; } = 1.0;

        [JsonProperty("overwrite")]
        public bool Overwrite { get; set; } = true;

        [JsonProperty("commitWithin")]
        public int CommitWithin { get; set; } = 1000;
    }

    public class AddRequest : BaseRequest
    {
        [JsonProperty("add")]
        public Add Add { get; set; }
    }

    public class AddRequestWithCommit : AddRequest
    {
        [JsonProperty("commit")]
        public object Commit { get; set; } = new object();
    }
}