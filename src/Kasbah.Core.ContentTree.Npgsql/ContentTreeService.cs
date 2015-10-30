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

        protected override IEnumerable<Tuple<T, DateTime>> InternalGetAllItemVersions<T>(Guid id)
        {
            const string ResourceName = "Sql/GetAllItemVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id });

                return data.Select(ent => new Tuple<T, DateTime>(Deserialise<T>(ent.Data), ent.Modified));
            }
        }

        protected override T InternalGetMostRecentlyCreatedItemVersion<T>(Guid id)
        {
            const string ResourceName = "Sql/GetMostRecentlyCreatedItemVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id });

                return data.Select(ent => Deserialise<T>(ent.Data)).First();
            }
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