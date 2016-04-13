using System;
using System.Collections.Generic;
using Kasbah.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;
using static Kasbah.Core.Tree.Sqlite.Utils.SerialisationUtil;

namespace Kasbah.Core.Tree.Sqlite
{
    public class SqliteTreeProvider : ITreeProvider, IDisposable
    {
        #region Private Members

        readonly SqliteConnection _connection;
        readonly ILogger _log;

        #endregion

        #region Public Constructors

        public SqliteTreeProvider(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<SqliteTreeProvider>();

            var connectionString = Environment.GetEnvironmentVariable("KASBAH_DB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _connection = new SqliteConnection(connectionString);
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void CreateNode(Guid id, Guid? parent, string alias, string type)
            => _connection.ExecuteFromResource("CreateNode", new { id, parent, alias, type });

        /// <inheritdoc/>
        public void Delete(Guid id)
            => _connection.ExecuteFromResource("Delete", new { id });

        /// <inheritdoc/>
        public void Dispose()
        {
            _connection.Dispose();
        }

        /// <inheritdoc/>
        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
            => _connection.QueryFromResource<NodeVersion>("GetAllNodeVersions", new { id });

        /// <inheritdoc/>
        public Node GetChild(Guid? parent, string alias)
            => _connection.QuerySingleFromResource<Node>("GetChild", new { parent, alias });

        /// <inheritdoc/>
        public IEnumerable<Node> GetChildren(Guid? id)
            => _connection.QueryFromResource<Node>("GetChildren", new { id });

        /// <inheritdoc/>
        public Node GetNode(Guid id)
            => _connection.QuerySingleFromResource<Node>("GetNode", new { id });

        /// <inheritdoc/>
        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
        {
            var data = GetRawNodeVersion(id, version);

            return data == null ? null : Deserialise(data.Data);
        }

        /// <inheritdoc/>
        public NodeVersion GetRawNodeVersion(Guid id, Guid version)
            => _connection.QuerySingleFromResource<NodeVersion>("GetNodeVersion", new { id, version });

        /// <inheritdoc/>
        public void MoveNode(Guid id, Guid? parent)
            => _connection.ExecuteFromResource("MoveNode", new { id, parent });

        /// <inheritdoc/>
        public NodeVersion Save(Guid id, Guid node, IDictionary<string, object> item)
        {
            var data = Serialise(item);

            return _connection.QuerySingleFromResource<NodeVersion>("Save", new { id, node, data });
        }

        /// <inheritdoc/>
        public void SetActiveNodeVersion(Guid id, Guid? version)
            => _connection.ExecuteFromResource("SetActiveNodeVersion", new { id, version });

        #endregion
    }
}