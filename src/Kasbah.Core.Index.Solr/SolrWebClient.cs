using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Kasbah.Core.Index.Solr.Models;
using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr
{
    public class SolrWebClient : WebClient
    {
        #region Public Constructors

        public SolrWebClient(Uri baseUri)
        {
            BaseAddress = baseUri.ToString();
        }

        #endregion

        #region Public Methods

        public void Commit()
            => SubmitUpdate(new CommitRequest());

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

        #endregion

        #region Private Fields

        const string CoreName = "kasbah";

        readonly Uri _selectUri = new Uri($"/solr/{CoreName}/select", UriKind.Relative);
        readonly Uri _updateUri = new Uri($"/solr/{CoreName}/update?wt=json", UriKind.Relative);

        #endregion
    }

    public class WebClient : IDisposable
    {
        #region Public Constructors

        public WebClient()
        {
        }

        #endregion

        #region Public Properties

        public string BaseAddress { get; set; }

        public string ContentType { get; set; }

        #endregion

        #region Public Methods

        public void Dispose()
        {
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

        #endregion
    }

    public class WebException : Exception
    {
    }
}