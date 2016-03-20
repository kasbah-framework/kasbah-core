using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace Kasbah.Core.Tree.Npgsql.Logging
{
    public class KasbahNpgsqlLoggingProvider : INpgsqlLoggingProvider
    {
        #region Public Constructors

        public KasbahNpgsqlLoggingProvider(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        #endregion

        #region Public Methods

        public NpgsqlLogger CreateLogger(string name)
        {
            return new KasbahNpgsqlLogger(_loggerFactory, name);
        }

        #endregion

        #region Private Fields

        readonly ILoggerFactory _loggerFactory;

        #endregion
    }
}