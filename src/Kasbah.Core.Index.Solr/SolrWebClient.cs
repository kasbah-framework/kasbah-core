using System;
using System.Collections.Generic;
using System.Net;
using Kasbah.Core.Index.Solr.Requests;
using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr
{
    public class SolrWebClient : WebClient
    {
        readonly Uri _updateUri = new Uri("/solr/kasbah/update?wt=json", UriKind.Relative);

        public SolrWebClient(Uri baseUri)
        {
            BaseAddress = baseUri.ToString();
        }

        public BaseResponse SubmitRequest(Uri uri, BaseRequest request)
        {
            Headers.Set("Content-Type", "application/json");
            
            var data = JsonConvert.SerializeObject(request);

            try
            {
                var ret = UploadString(uri, data);

                return JsonConvert.DeserializeObject<BaseResponse>(ret);
            }
            catch (WebException ex)
            {
                throw new Exception($"Invalid request: {uri} -- {data}", ex);
            }
        }

        public void SubmitUpdate(BaseRequest request)
        {
            var uri = new Uri(new Uri(BaseAddress), _updateUri);

            var response = SubmitRequest(uri, request);

            if (response.Status.Code != 0)
            {
                throw new InvalidOperationException();
            }
        }

        public void InsertOrUpdate(Guid id, IDictionary<string, object> data)
        {
            data["id"] = id;

            var request = new AddRequest
            {
                Add = new Add
                {
                    Document = SolrUtil.ConvertToSolr(data)
                }
            };

            SubmitUpdate(request);
        }

        public void Delete(Guid id)
            => SubmitUpdate(new DeleteRequest { Delete = new Delete { Id = id.ToString() } });

        public void Commit() => SubmitUpdate(new CommitRequest());
    }
}