using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr.Requests
{
    // {"add":{ "doc":{"id":"change.me","title":"change.me"},"boost":1.0,"overwrite":true,"commitWithin":1000}}

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

    public class BaseRequest { }

    public class BaseResponse
    {
        [JsonProperty("responseHeader")]
        public ResponseStatus Status { get; set; }
    }

    public class ResponseStatus
    {
        [JsonProperty("status")]
        public int Code { get; set; }

        [JsonProperty("QTime")]
        public int QueryTime { get; set; }
    }

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

    public class CommitRequest : BaseRequest
    {
        [JsonProperty("commit")]
        public object Commit { get; set; } = new object();
    }

    public class SelectResponse : BaseResponse
    {
        [JsonProperty("response")]
        public SelectResponseBody Response { get; set; }
    }

    public class SelectResponseBody
    {
        [JsonProperty("numFound")]
        public int TotalCount { get; set; }

        [JsonProperty("start")]
        public int Offset { get; set; }

        [JsonProperty("docs")]
        public IEnumerable<IDictionary<string, object>> Documents { get; set; }
    }
}