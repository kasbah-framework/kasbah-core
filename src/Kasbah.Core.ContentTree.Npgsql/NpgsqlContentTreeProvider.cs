using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.ContentTree.Npgsql.Models;
using Kasbah.Core.Models;
using Npgsql;
using static Kasbah.Core.ContentTree.Npgsql.Utils.SerialisationUtil;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public class NpgsqlContentTreeProvider : IContentTreeProvider
    {
        #region Public Methods

        public void CreateNode(Guid id, Guid? parent, string alias, string type)
        {
            using (var connection = GetConnection())
            {
                connection.ExecuteFromResource("CreateNode", new { id, parent, alias, type });
            }
        }

        public T GetActiveNodeVersion<T>(Guid id)
            where T : ItemBase
        {
            using (var connection = GetConnection())
            {
                var data = connection.QuerySingleFromResource<NpgsqlNodeVersion>("GetActiveNodeVersion", new { id });

                return data == null ? null : Deserialise<T>(data.Data);
            }
        }

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryFromResource<NpgsqlNodeVersion>("GetAllNodeVersions", new { id });
            }
        }

        public Node GetChild(Guid? parent, string alias)
        {
            using (var connection = GetConnection())
            {
                return connection.QuerySingleFromResource<Node>("GetChild", new { parent, alias });
            }
        }

        public IEnumerable<Node> GetChildren(Guid? id)
        {
            using (var connection = GetConnection())
            {
                return connection.QueryFromResource<Node>("GetChildren", new { id });
            }
        }

        public Node GetNode(Guid id)
        {
            using (var connection = GetConnection())
            {
                return connection.QuerySingleFromResource<Node>("GetNode", new { id });
            }
        }

        public object GetNodeVersion(Guid id, Guid version, Type type)
        {
            using (var connection = GetConnection())
            {
                var data = connection.QuerySingleFromResource<NpgsqlNodeVersion>("GetNodeVersion", new { id, version });

                return data == null ? null : Deserialise(data.Data, type);
            }
        }

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
        {
            using (var connection = GetConnection())
            {
                var data = connection.QuerySingleFromResource<NpgsqlNodeVersion>("GetNodeVersion", new { id, version });

                return data == null ? null : DeserialiseAnonymous(data.Data);
            }
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            using (var connection = GetConnection())
            {
                connection.ExecuteFromResource("MoveNode", new { id, parent });
            }
        }

        public NodeVersion Save<T>(Guid id, Guid node, T item) where T : ItemBase
        {
            return Save(id, node, (object)item);
        }

        public NodeVersion Save(Guid id, Guid node, object item)
        {
            var data = Serialise(item);

            using (var connection = GetConnection())
            {
                return connection.QuerySingleFromResource<NodeVersion>("Save", new { id, node, data });
            }
        }

        public void SetActiveNodeVersion(Guid id, Guid? version)
        {
            using (var connection = GetConnection())
            {
                connection.ExecuteFromResource("SetActiveNodeVersion", new { id, version });
            }
        }

        #endregion

        #region Private Methods

        NpgsqlConnection GetConnection()
        {
            var connectionString = Environment.GetEnvironmentVariable("DB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            return new NpgsqlConnection(connectionString);
        }

        #endregion
    }
}