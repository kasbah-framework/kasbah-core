using System.Collections.Generic;
using System;
using System.Linq;

namespace Kasbah.Core.Index.Solr
{
    public class SolrIndexProvider : IIndexProvider
    {
        #region Public Methods

        public IEnumerable<IDictionary<string, object>> Query(object query)
        {
            using (var connection = GetConnection())
            {
                var solrQuery = ParseQuery(query);

                var response = connection.Select(ParseQuery(query));

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
                connection.InsertOrUpdate((Guid)value["id"], value);
            }
        }

        public void Delete(Guid id)
        {
            using (var connection = GetConnection())
            {
                connection.Delete(id);
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

            return new SolrWebClient(new Uri(connectionString, UriKind.Absolute));
        }

        string ParseQuery(object query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var type = query.GetType();
            var props = type.GetProperties();

            var ret = new Dictionary<string, object>() as IDictionary<string, object>;

            foreach (var prop in props)
            {
                var value = prop.GetValue(query);

                ret.Add(prop.Name, value);
            }

            ret = SolrUtil.ConvertToSolr(ret);

            return string.Join(" ", ret.Select(ent => $"{ent.Key}:{ent.Value}"));
        }

        #endregion
    }
}