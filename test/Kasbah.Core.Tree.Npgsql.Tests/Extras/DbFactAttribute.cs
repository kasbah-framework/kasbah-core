using System;
using Xunit;

namespace Kasbah.Core.Tree.Npgsql.Tests
{
    public class DbFactAttribute : FactAttribute
    {
        #region Public Constructors

        public DbFactAttribute()
        {
            if (Environment.GetEnvironmentVariable("KASBAH_DB") == null)
            {
                Skip = "Database unavailable";
            }
        }

        #endregion
    }
}