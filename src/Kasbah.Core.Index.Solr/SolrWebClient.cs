using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Kasbah.Core.Index.Solr.Models;
using System.IO;

namespace Kasbah.Core.Index.Solr
{

    public class WebClient : IDisposable
    {
        public string BaseAddress { get; set; }

        public string ContentType { get; set; }

        public WebClient()
        {
        }

        public string UploadString(Uri uri, string data)
        {
            var request = HttpWebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = ContentType;

            using (var stream = request.GetRequestStreamAsync().Result)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(data);
                stream.Write(bytes, 0, bytes.Length);
            }

            using (var response = request.GetResponseAsync().Result)
            {
                var realResponse = response as HttpWebResponse;
                using (var stream = realResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public string DownloadString(Uri uri)
        {
            var request = HttpWebRequest.Create(uri);
            request.Method = "GET";

            using (var response = request.GetResponseAsync().Result)
            {
                var realResponse = response as HttpWebResponse;
                using (var stream = realResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }

    public class WebException : Exception
    {

    }


    public class SolrWebClient : WebClient
    {
        const string CoreName = "kasbah";

        readonly Uri _updateUri = new Uri($"/solr/{CoreName}/update?wt=json", UriKind.Relative);
        readonly Uri _selectUri = new Uri($"/solr/{CoreName}/select", UriKind.Relative);

        public SolrWebClient(Uri baseUri)
        {
            BaseAddress = baseUri.ToString();
        }

        public BaseResponse SubmitRequest(Uri uri, BaseRequest request)
        {
            ContentType = "application/json";

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

        public void InsertOrUpdate(IDictionary<string, object> data)
        {
            var request = new AddRequestWithCommit
            {
                Add = new Add
                {
                    Document = SolrUtil.ConvertToSolr(data)
                }
            };

            SubmitUpdate(request);
        }

        public SelectResponse Select(string query)
        {
            var baseUri = new Uri(new Uri(BaseAddress), _selectUri);
            var uriBuilder = new UriBuilder(baseUri);

            uriBuilder.Query = $"wt=json&q={query}";

            var data = DownloadString(uriBuilder.Uri);

            return JsonConvert.DeserializeObject<SelectResponse>(data);
        }

        public void Delete(Guid id)
        {
            var request = new DeleteRequestWithCommit
            {
                Delete = new Delete
                {
                    Id = id.ToString()
                }
            };

            SubmitUpdate(request);
        }

        public void Commit()
            => SubmitUpdate(new CommitRequest());
    }
}