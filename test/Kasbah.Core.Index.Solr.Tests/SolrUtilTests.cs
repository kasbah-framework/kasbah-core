using System;
using Kasbah.Core.Index.Utils;
using Xunit;

namespace Kasbah.Core.Index.Solr.Tests
{
    public class SolrUtilTests
    {
        [Fact]
        public void SolrUtil_ObjectWithInts_HasCorrectKeyForSolr()
        {
            // Arrange
            var obj = new
            {
                A = (int)1,
                B = "B",
                C = DateTime.Now,
                D = double.MaxValue,
                E = true
            };

            var dict = SerialisationUtil.Serialise(obj);

            // Act
            var solrDict = SolrUtil.ConvertToSolr(dict);

            // Assert
            Assert.NotNull(solrDict);
            Assert.True(solrDict.ContainsKey("a_i"), "integer");
            Assert.True(solrDict.ContainsKey("b_t"), "string");
            Assert.True(solrDict.ContainsKey("c_dt"), "datetime");
            Assert.True(solrDict.ContainsKey("d_d"), "double");
            Assert.True(solrDict.ContainsKey("e_b"), "boolean");
        }
    }
}