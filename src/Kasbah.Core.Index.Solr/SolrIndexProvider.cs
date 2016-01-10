using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Kasbah.Core.Index.Solr
{
    public class SolrIndexProvider : IIndexProvider
    {
        readonly ILoggerFactory _loggerFactory;
        public SolrIndexProvider(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        #region Public Methods

        public void Delete(Guid id)
        {
            using (var connection = GetConnection())
            {
                connection.Delete(id);
            }
        }

        public IEnumerable<IDictionary<string, object>> Query(object query, int? limit = null, string sort = null)
        {
            using (var connection = GetConnection())
            {
                var solrQuery = ParseQuery(query);

                var response = connection.Select(ParseQuery(query), limit, sort);

                return response.Response.Documents.Select(SolrUtil.ConverFromSolr);
            }
        }

        public void Store(IDictionary<string, object> value)
        {
            if (!value.ContainsKey("id"))
            {
                throw new InvalidOperationException("Id field missing");
            }

            using (var connection = GetConnection())
            {
                connection.InsertOrUpdate(value);
            }
        }

        #endregion

        #region Private Methods

        SolrWebClient GetConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("SOLR");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var baseUri = connectionString.Split(';').First();
            var coreName = connectionString.Split(';').Last();

            return new SolrWebClient(new Uri(baseUri, UriKind.Absolute), coreName, _loggerFactory);
        }

        string ParseQuery(object query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            if (query is string)
            {
                var terms = (query as string).Split(' ', '-');

                return string.Join(" ", terms.Select(s => $"_text_:\"{s}\""));
            }
            else
            {
                var typeInfo = query.GetType().GetTypeInfo();
                var props = typeInfo.DeclaredProperties;

                var ret = new Dictionary<string, object>() as IDictionary<string, object>;

                foreach (var prop in props)
                {
                    var value = prop.GetValue(query);

                    ret.Add(prop.Name, value);
                }

                ret = SolrUtil.ConvertToSolr(ret);

                return string.Join(" ", ret.Select(ent => $"{ent.Key}:\"{ent.Value}\""));
            }
        }

        #endregion
    }
}