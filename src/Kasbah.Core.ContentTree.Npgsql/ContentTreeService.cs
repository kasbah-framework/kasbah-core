using System;
using System.Linq;
using Npgsql;
using Dapper;
using System.Collections.Generic;
using Kasbah.Core.Utils;
using Kasbah.Core.Events;

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
            return new NpgsqlConnection();
        }

        protected override IEnumerable<Tuple<T, DateTime>> InternalGetAllItemVersions<T>(Guid id)
        {
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.GetAllItemVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NodeVersion>(sql, new { id });

                return data.Select(ent => new Tuple<T, DateTime>(DeserialiseObject<T>(ent.Data), ent.Timestamp));
            }
        }

        protected override T InternalGetMostRecentlyCreatedItemVersion<T>(Guid id)
        {
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.GetMostRecentlyCreatedItemVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NodeVersion>(sql, new { id });

                return data.Select(ent => DeserialiseObject<T>(ent.Data)).First();
            }
        }

        protected override void InternalSave<T>(Guid id, T item)
        {
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.Save.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);
            var data = SerialiseObject(item);

            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { id, data });
            }
        }

        byte[] SerialiseObject(object input)
        {
            return null;
        }

        T DeserialiseObject<T>(byte[] input)
        {
            return default(T);
        }

        class Node
        {
            public Guid Id { get; set; }

            public string Alias { get; set; }
        }

        class NodeVersion
        {
            public Guid Id { get; set; }
            public Guid NodeId { get; set; }
            public DateTime Timestamp { get; set; }
            public byte[] Data { get; set; }
        }
    }
}