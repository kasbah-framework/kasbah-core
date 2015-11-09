using Npgsql;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Kasbah.Core.Utils;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public static class NpgsqlConnectionExtensions
    {
        public static void ExecuteFromResource(this NpgsqlConnection connection, string resourceName, object parameters = null)
        {
            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>($"Sql/{resourceName}.sql");

            connection.Execute(sql, parameters);
        }

        public static IEnumerable<T> QueryFromResource<T>(this NpgsqlConnection connection, string resourceName, object parameters = null)
        {
            var sql = ResourceUtil.Get<NpgsqlContentTreeProvider>($"Sql/{resourceName}.sql");

            return connection.Query<T>(sql, parameters);
        }

        public static T QuerySingleFromResource<T>(this NpgsqlConnection connection, string resourceName, object parameters = null)
        {
            return QueryFromResource<T>(connection, resourceName, parameters).SingleOrDefault();
        }
    }
}