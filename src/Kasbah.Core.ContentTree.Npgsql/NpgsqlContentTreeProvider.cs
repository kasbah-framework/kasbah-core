using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.ContentTree.Npgsql.Models;
using Kasbah.Core.Events;
using Kasbah.Core.Models;
using Kasbah.Core.Utils;
using Npgsql;
using static Kasbah.Core.ContentTree.Npgsql.Utils.SerialisationUtil;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public class NpgsqlContentTreeProvider : IContentTreeProvider
    {
        #region Public Methods

        public void CreateNode(Guid id, Guid? parent, string alias, string type)
        {
            const string ResourceName = "Sql/CreateNode.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { id, parent, alias, type });
            }
        }

        public T GetActiveNodeVersion<T>(Guid id)
                    where T : ItemBase
        {
            const string ResourceName = "Sql/GetActiveNodeVersion.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id });

                return data.Select(ent => Deserialise<T>(ent.Data)).SingleOrDefault();
            }
        }

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            const string ResourceName = "Sql/GetAllNodeVersions.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<NpgsqlNodeVersion>(sql, new { id });
            }
        }

        public Node GetChild(Guid? parent, string alias)
        {
            const string ResourceName = "Sql/GetChild.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<Node>(sql, new { parent, alias }).SingleOrDefault();
            }
        }

        public IEnumerable<Node> GetChildren(Guid? id)
        {
            const string ResourceName = "Sql/GetChildren.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<Node>(sql, new { id });
            }
        }

        public Node GetNode(Guid id)
        {
            const string ResourceName = "Sql/GetNode.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<Node>(sql, new { id }).SingleOrDefault();
            }
        }

        public object GetNodeVersion(Guid id, Guid version, Type type)
        {
            const string ResourceName = "Sql/GetNodeVersion.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id, version });

                return data.Select(ent => Deserialise(ent.Data, type)).SingleOrDefault();
            }
        }

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
        {
            const string ResourceName = "Sql/GetNodeVersion.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id, version });

                return data.Select(ent => DeserialiseAnonymous(ent.Data)).SingleOrDefault();
            }
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            const string ResourceName = "Sql/MoveNode.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { id, parent });
            }
        }

        public NodeVersion Save<T>(Guid id, Guid node, T item) where T : ItemBase
        {
            return Save(id, node, (object)item);
        }

        public NodeVersion Save(Guid id, Guid node, object item)
        {
            const string ResourceName = "Sql/Save.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);
            var data = Serialise(item);

            using (var connection = GetConnection())
            {
                return connection.Query<NodeVersion>(sql, new { id, node, data }).SingleOrDefault();
            }
        }

        public void SetActiveNodeVersion(Guid id, Guid version)
        {
            const string ResourceName = "Sql/SetActiveNodeVersion.sql";

            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>(ResourceName);

            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { id, version });
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