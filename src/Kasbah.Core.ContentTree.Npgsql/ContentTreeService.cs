using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.ContentTree.Npgsql.Models;
using Kasbah.Core.Events;
using Kasbah.Core.Utils;
using Npgsql;
using static Kasbah.Core.ContentTree.Npgsql.Utils.SerialisationUtil;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public class ContentTreeService : ContentTreeServiceBase, IContentTreeService
    {
        #region Public Constructors

        public ContentTreeService(IEventService eventService)
            : base(eventService)
        {
        }

        #endregion

        #region Public Methods

        public override IEnumerable<Node> GetChildren(Guid? id)
        {
            const string ResourceName = "Sql/GetChildren.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<Node>(sql, new { id });
            }
        }

        public override IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            const string ResourceName = "Sql/GetAllNodeVersions.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<NpgsqlNodeVersion>(sql, new { id });
            }
        }

        public override T GetMostRecentlyCreatedNodeVersion<T>(Guid id)
        {
            const string ResourceName = "Sql/GetMostRecentlyCreatedNodeVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id });

                return data.Select(ent => Deserialise<T>(ent.Data)).First();
            }
        }

        public override Node GetNode(Guid id)
        {
            const string ResourceName = "Sql/GetNode.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<Node>(sql, new { id }).First();
            }
        }

        public override T GetActiveNodeVersion<T>(Guid id)
        {
            const string ResourceName = "Sql/GetActiveNodeVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id });

                return data.Select(ent => Deserialise<T>(ent.Data)).First();
            }
        }

        public override T GetNodeVersion<T>(Guid id, Guid version)
        {
            const string ResourceName = "Sql/GetNodeVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id, version });

                return data.Select(ent => Deserialise<T>(ent.Data)).First();
            }
        }

        public override object GetNodeVersion(Guid id, Guid version, Type type)
        {
            const string ResourceName = "Sql/GetNodeVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id, version });

                return data.Select(ent => Deserialise(ent.Data, type)).First();
            }
        }

        #endregion

        #region Protected Methods

        protected override Guid InternalCreateNode(Guid? parent, string alias)
        {
            var id = Guid.NewGuid();

            const string ResourceName = "Sql/CreateNode.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { id, parent, alias });
            }

            return id;
        }

        protected override NodeVersion InternalSave<T>(Guid id, Guid node, T item)
        {
            const string ResourceName = "Sql/Save.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);
            var data = Serialise(item);

            using (var connection = GetConnection())
            {
                return connection.Query<NodeVersion>(sql, new { id, node, data }).FirstOrDefault();
            }
        }

        #endregion

        #region Private Methods

        NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(Environment.GetEnvironmentVariable("DB"));
        }

        #endregion
    }
}
