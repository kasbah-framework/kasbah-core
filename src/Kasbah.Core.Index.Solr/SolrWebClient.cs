using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Kasbah.Core.Index.Solr.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kasbah.Core.Index.Solr
{
    public class SolrWebClient : WebClient
    {
        #region Public Constructors

        public SolrWebClient(Uri baseUri, string coreName, ILoggerFactory loggerFactory)
        {
            BaseAddress = baseUri.ToString();

            _coreName = coreName;
            _log = loggerFactory.CreateLogger<SolrWebClient>();
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

        public SelectResponse Select(string query, int? start, int? rows, string sort)
        {
            var baseUri = new Uri(new Uri(BaseAddress), _selectUri);
            var uriBuilder = new UriBuilder(baseUri);

            var queryString = $"wt=json&q={query}";

            if (start.HasValue)
            {
                queryString += $"&start={start}";
            }
            if (rows.HasValue)
            {
                queryString += $"&rows={rows}";
            }
            if (!string.IsNullOrEmpty(sort))
            {
                queryString += $"&sort={sort}";
            }

            uriBuilder.Query = queryString;

            _log.LogVerbose($"Select request: {uriBuilder.Uri}");

            var data = DownloadString(uriBuilder.Uri);

            _log.LogVerbose($"Select response: {data}");

            return JsonConvert.DeserializeObject<SelectResponse>(data);
        }

        public BaseResponse SubmitRequest(Uri uri, BaseRequest request)
        {
            ContentType = "application/json";

            var data = JsonConvert.SerializeObject(request);

            _log.LogVerbose($"SubmitRequest data: {data}");

            try
            {
                var ret = UploadString(uri, data);

                _log.LogVerbose($"SubmitRequest response: {ret}");

                return JsonConvert.DeserializeObject<BaseResponse>(ret);
            }
            catch (WebException ex)
            {
                throw new Exception($"Invalid request: {uri} -- {data}", ex);
            }
            catch (AggregateException ex)
            {
                var wex = (ex.InnerExceptions.FirstOrDefault(ent => ent is WebException));
                if (wex != null)
                {
                    throw new Exception($"Invalid request: {uri} -- {data}", ex);
                }

                throw ex;
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

        readonly string _coreName;

        Uri _selectUri => new Uri($"/solr/{_coreName}/select", UriKind.Relative);
        Uri _updateUri => new Uri($"/solr/{_coreName}/update?wt=json", UriKind.Relative);

        readonly ILogger _log;

        #endregion

#if !DNXCORE50
        internal string ContentType { set { Headers[HttpRequestHeader.ContentType] = value; } }
#endif
    }

#if DNXCORE50
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
#endif
}