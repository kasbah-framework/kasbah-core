using System;
using System.Linq;
using Npgsql;
using Dapper;
using System.Collections.Generic;
using Kasbah.Core.Utils;
using Kasbah.Core.Events;
using Kasbah.Core.ContentTree.Npgsql.Models;
using static Kasbah.Core.ContentTree.Npgsql.Utils.SerialisationUtil;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public class ContentTreeService : ContentTreeServiceBase, IContentTreeService
    {
        public ContentTreeService(IEventService eventService)
            : base(eventService)
        {

        }

        NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(Environment.GetEnvironmentVariable("DB"));
        }

        protected override IEnumerable<Tuple<T, DateTime>> InternalGetAllItemVersions<T>(Guid id)
        {
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.GetAllItemVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NodeVersion>(sql, new { id });

                return data.Select(ent => new Tuple<T, DateTime>(Deserialise<T>(ent.Data), ent.Timestamp));
            }
        }

        protected override T InternalGetMostRecentlyCreatedItemVersion<T>(Guid id)
        {
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.GetMostRecentlyCreatedItemVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NodeVersion>(sql, new { id });

                return data.Select(ent => Deserialise<T>(ent.Data)).First();
            }
        }

        protected override void InternalSave<T>(Guid id, T item)
        {
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.Save.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);
            var data = Serialise(item);

            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { id, data });
            }
        }
    }
}