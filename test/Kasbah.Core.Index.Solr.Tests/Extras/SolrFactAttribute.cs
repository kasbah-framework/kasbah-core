using System;
using Xunit;

namespace Kasbah.Core.Index.Solr.Tests
{
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
    public class SolrDbFactAttribute : FactAttribute
    {
        #region Public Constructors

        public SolrDbFactAttribute()
        {
            Skip = null;

            if (Environment.GetEnvironmentVariable("SOLR") == null)
            {
                Skip = "Solr";
            }

            if (Environment.GetEnvironmentVariable("DB") == null)
            {
                if (Skip == null)
                {
                    Skip = "Database";
                }
                else
                {
                    Skip += " and database";
                }
            }

            if (Skip != null)
            {
                Skip += " unavailable";
            }
        }

        #endregion
    }
}