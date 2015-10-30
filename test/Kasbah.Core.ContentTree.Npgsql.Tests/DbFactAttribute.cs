using System;
using Xunit;

namespace Kasbah.Core.ContentTree.Npgsql.Tests
{
    public class DbFactAttribute : FactAttribute
    {
        #region Public Constructors

        public DbFactAttribute()
        {
            if (Environment.GetEnvironmentVariable("DB") == null)
            {
                Skip = "Database unavailable";
            }
        }

        #endregion
    }
}