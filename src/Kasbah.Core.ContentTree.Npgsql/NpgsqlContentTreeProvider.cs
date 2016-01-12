using System;
using System.Collections.Generic;
using System.Data;
using Kasbah.Core.Models;
using Kasbah.Core.Utils;
using Microsoft.Extensions.Logging;
using Npgsql;
using static Kasbah.Core.ContentTree.Npgsql.Utils.SerialisationUtil;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public class NpgsqlContentTreeProvider : IContentTreeProvider, IDisposable
    {
        #region Private Members

        readonly NpgsqlConnection _connection;
        readonly ILogger _log;

        #endregion

        #region Public Constructors

        public NpgsqlContentTreeProvider(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<NpgsqlContentTreeProvider>();

            var connectionString = Environment.GetEnvironmentVariable("DB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connection = new NpgsqlConnection(connectionString);

            EnsureSchema();
        }

        #endregion

        #region Public Methods

        public void CreateNode(Guid id, Guid? parent, string alias, string type)
        {
            _connection.ExecuteFromResource("CreateNode", new { id, parent, alias, type });
        }

        public void Dispose()
        {
        }

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            return _connection.QueryFromResource<NodeVersion>("GetAllNodeVersions", new { id });
        }

        public Node GetChild(Guid? parent, string alias)
        {
            return _connection.QuerySingleFromResource<Node>("GetChild", new { parent, alias });
        }

        public IEnumerable<Node> GetChildren(Guid? id)
        {
            return _connection.QueryFromResource<Node>("GetChildren", new { id });
        }

        public Node GetNode(Guid id)
        {
            return _connection.QuerySingleFromResource<Node>("GetNode", new { id });
        }

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
        {
            var data = GetRawNodeVersion(id, version);

            return data == null ? null : Deserialise(data.Data);
        }

        public NodeVersion GetRawNodeVersion(Guid id, Guid version)
        {
            return _connection.QuerySingleFromResource<NodeVersion>("GetNodeVersion", new { id, version });
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            _connection.ExecuteFromResource("MoveNode", new { id, parent });
        }

        public NodeVersion Save(Guid id, Guid node, IDictionary<string, object> item)
        {
            var data = Serialise(item);

            return _connection.QuerySingleFromResource<NodeVersion>("Save", new { id, node, data });
        }

        public void SetActiveNodeVersion(Guid id, Guid? version)
        {
            _connection.ExecuteFromResource("SetActiveNodeVersion", new { id, version });
        }

        #endregion

        #region Private methods

        void EnsureSchema()
        {
            _connection.ExecuteFromResource("Schema.fn");
            _connection.ExecuteFromResource("Schema");

        }

        #endregion
    }
}