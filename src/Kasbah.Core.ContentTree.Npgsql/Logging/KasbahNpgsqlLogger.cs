using System;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace Kasbah.Core.ContentTree.Npgsql.Logging
{
    public class KasbahNpgsqlLogger : NpgsqlLogger
    {
        #region Public Constructors

        public KasbahNpgsqlLogger(ILoggerFactory loggerFactory, string name)
        {
            _log = loggerFactory.CreateLogger<KasbahNpgsqlLogger>();
            _name = name;
        }

        #endregion

        #region Private Fields

        readonly ILogger _log;
        readonly string _name;

        #endregion

        #region Public Methods

        public override bool IsEnabled(NpgsqlLogLevel level)
        {
            return _log.IsEnabled(MapLevel(level));
        }

        public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception exception = null)
        {
            _log.Log(MapLevel(level), connectorId, this, exception, (m, ex) => $"{_name} {level}  {msg} {ex}");
        }

        #endregion

        #region Private Methods

        static LogLevel MapLevel(NpgsqlLogLevel level)
        {
            switch (level)
            {
                case NpgsqlLogLevel.Debug:
                    return LogLevel.Debug;

                case NpgsqlLogLevel.Error:
                    return LogLevel.Error;

                case NpgsqlLogLevel.Fatal:
                    return LogLevel.Critical;

                case NpgsqlLogLevel.Info:
                    return LogLevel.Information;

                case NpgsqlLogLevel.Trace:
                    return LogLevel.Information;

                case NpgsqlLogLevel.Warn:
                    return LogLevel.Warning;
            }

            throw new ArgumentOutOfRangeException(nameof(level));
        }

        #endregion
    }
}