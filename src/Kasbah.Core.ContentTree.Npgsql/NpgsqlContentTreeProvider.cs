using System;
using System.Collections.Generic;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;
using Npgsql;
using static Kasbah.Core.ContentTree.Npgsql.Utils.SerialisationUtil;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public class NpgsqlContentTreeProvider : IContentTreeProvider, IDisposable
    {
        #region Private Members

#if ENABLE_SCHEMA_CHECK
        static bool FirstRun = true;
#endif
        readonly NpgsqlConnection _connection;
        readonly object _lock = new object();
        readonly ILogger _log;

        #endregion

        #region Public Constructors

        public NpgsqlContentTreeProvider(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<NpgsqlContentTreeProvider>();

            var connectionString = Environment.GetEnvironmentVariable("KASBAH_DB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connection = new NpgsqlConnection(connectionString);
        }

        #endregion

        #region Public Methods

        public void CreateNode(Guid id, Guid? parent, string alias, string type)
        {
            lock (_lock)
            {
                _connection.ExecuteFromResource("CreateNode", new { id, parent, alias, type });
            }
        }

        public void Delete(Guid id)
        {
            lock (_lock)
            {
                _connection.ExecuteFromResource("Delete", new { id });
            }
        }

        public void Dispose()
        {
        }

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            lock (_lock)
            {
                return _connection.QueryFromResource<NodeVersion>("GetAllNodeVersions", new { id });
            }
        }

        public Node GetChild(Guid? parent, string alias)
        {
            lock (_lock)
            {
                return _connection.QuerySingleFromResource<Node>("GetChild", new { parent, alias });
            }
        }

        public IEnumerable<Node> GetChildren(Guid? id)
        {
            lock (_lock)
            {
                return _connection.QueryFromResource<Node>("GetChildren", new { id });
            }
        }

        public Node GetNode(Guid id)
        {
            lock (_lock)
            {
                return _connection.QuerySingleFromResource<Node>("GetNode", new { id });
            }
        }

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
        {
            lock (_lock)
            {
                var data = GetRawNodeVersion(id, version);

                return data == null ? null : Deserialise(data.Data);
            }
        }

        public NodeVersion GetRawNodeVersion(Guid id, Guid version)
        {
            lock (_lock)
            {
                return _connection.QuerySingleFromResource<NodeVersion>("GetNodeVersion", new { id, version });
            }
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            lock (_lock)
            {
                _connection.ExecuteFromResource("MoveNode", new { id, parent });
            }
        }

        public NodeVersion Save(Guid id, Guid node, IDictionary<string, object> item)
        {
            lock (_lock)
            {
                var data = Serialise(item);

                return _connection.QuerySingleFromResource<NodeVersion>("Save", new { id, node, data });
            }
        }

        public void SetActiveNodeVersion(Guid id, Guid? version)
        {
            lock (_lock)
            {
                _connection.ExecuteFromResource("SetActiveNodeVersion", new { id, version });
            }
        }

        #endregion
    }
}