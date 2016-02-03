using System;
using Xunit;

namespace Kasbah.Core.Index.Solr.Tests
{
    public class SolrDbFactAttribute : FactAttribute
    {
        #region Public Constructors

        public SolrDbFactAttribute()
        {
            if (Environment.GetEnvironmentVariable("SOLR") == null)
            {
                Skip = "Solr unavailable";
            }
        }

        #endregion
    }

    public class SolrFactAttribute : FactAttribute
    {
        #region Public Constructors

        public SolrFactAttribute()
        {
            if (Environment.GetEnvironmentVariable("SOLR") == null)
            {
                Skip = "Solr unavailable";
            }
        }

        #endregion
    }
}