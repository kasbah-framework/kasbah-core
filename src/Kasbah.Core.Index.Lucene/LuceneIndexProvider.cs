using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace Kasbah.Core.Index.Lucene
{
    public class LuceneIndexProvider : IIndexProvider
    {
        #region Public Constructors

        public LuceneIndexProvider()
        {
            _directory = GetDirectory();
            _analyser = new StandardAnalyzer(Version.LUCENE_30);
            _indexWriter = new IndexWriter(_directory, _analyser, IndexWriter.MaxFieldLength.UNLIMITED);
            _indexReader = DirectoryReader.Open(_directory, true);
        }

        #endregion

        #region Public Methods

        public void Delete(Guid id)
        {
            _indexWriter.DeleteDocuments(new Term("id", id.ToString()));
        }

        public IEnumerable<IDictionary<string, object>> Query(object query, int? skip = null, int? take = null, string sort = null)
        {
            var luceneQuery = ParseQuery(query);

            var searcher = new IndexSearcher(_directory);

            var terms = luceneQuery.Select(ent => new TermQuery(new Term(ent.Key, ent.Value.ToString())));

            // var parser = new QueryParser(Version.LUCENE_30, _analyser, "id");
            // var qry = parser.Parse(luceneQuery);

            // var results = searcher.Search(qry);

            // return results.Documents.Select(LuceneUtil.ConverFromSolr);

            throw new NotImplementedException();
        }

        public void Store(IDictionary<string, object> value)
        {
            if (!value.ContainsKey("id"))
            {
                throw new InvalidOperationException("Id field missing");
            }

            var doc = new Document();
            doc.Add(new Field("id", value["id"].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            foreach (var kvp in value)
            {
                var str = kvp.Value?.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    doc.Add(new Field(kvp.Key, str, Field.Store.YES, Field.Index.ANALYZED));
                }
            }

            _indexWriter.AddDocument(doc);
        }

        #endregion

        #region Private Methods

        Directory GetDirectory()
        {
            var path = Environment.GetEnvironmentVariable("LUCENE_PATH");

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            return FSDirectory.Open(path);
        }

        IDictionary<string, object> ParseQuery(object query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            if (query is string)
            {
                // var terms = (query as string).Split(' ', '-');

                // return string.Join(" ", terms.Select(s => $"_text_:\"{s}\""));

                throw new NotImplementedException();
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

                return LuceneUtil.ConvertToSolr(ret);
            }
        }

        #endregion

        #region Private members

        readonly StandardAnalyzer _analyser;
        readonly Directory _directory;
        readonly IndexReader _indexReader;
        readonly IndexWriter _indexWriter;

        #endregion
    }
}