using System;
using System.Linq;
using Kasbah.Core.Models;
using Npgsql;
using Dapper;
using System.Collections.Generic;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public class ContentTreeService : IContentTreeService
    {
        NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection();
        }

        public IEnumerable<Tuple<T, DateTime>> GetAllItemVersions<T>(Guid id)
        {
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.GetAllItemVersion.sql";

            var sql = Kasbah.Core.Utils.ResourceHelper.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NodeVersion>(sql, new { id });

                return data.Select(ent => new Tuple<T, DateTime>(DeserialiseObject<T>(ent.Data), ent.Timestamp));
            }
        }

        public T GetMostRecentlyCreatedItemVersion<T>(Guid id)
        {
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.GetMostRecentlyCreatedItemVersion.sql";

            var sql = Kasbah.Core.Utils.ResourceHelper.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NodeVersion>(sql, new { id });

                return data.Select(ent => DeserialiseObject<T>(ent.Data)).First();
            }
        }

        public void Save<T>(Guid id, T item) where T : ItemBase
        {
            // TODO: raise before save event
            const string ResourceName = "Kasbah.Core.ContentTree.Npgsql.Sql.Save.sql";

            var sql = Kasbah.Core.Utils.ResourceHelper.Get<ContentTreeService>(ResourceName);
            var data = SerialiseObject(item);

            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { id, data });
            }

            // TODO: raise after save event
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