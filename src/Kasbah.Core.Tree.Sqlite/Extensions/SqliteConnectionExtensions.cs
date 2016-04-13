using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Kasbah.Core.Utils;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace Kasbah.Core.Tree.Sqlite
{
    public static class SqliteConnectionExtensions
    {
        static SqliteConnectionExtensions()
        {
            SqlMapper.AddTypeHandler(new GuidExtHandler());
        }

        #region Public Methods

        public static void ExecuteFromResource(this SqliteConnection connection, string resourceName, object parameters = null)
        {
            var sql = ResourceUtil.Get<SqliteTreeProvider>($"Sql/{resourceName}.sql");

            connection.Execute(sql, parameters);
        }

        public static IEnumerable<T> QueryFromResource<T>(this SqliteConnection connection, string resourceName, object parameters = null)
        {
            var sql = ResourceUtil.Get<SqliteTreeProvider>($"Sql/{resourceName}.sql");

            return connection.Query<T>(sql, parameters);
        }

        public static T QuerySingleFromResource<T>(this SqliteConnection connection, string resourceName, object parameters = null)
        {
            return QueryFromResource<T>(connection, resourceName, parameters).SingleOrDefault();
        }

        #endregion
    }

    public class GuidExtHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(DbParameter parameter, Guid value)
        {
            parameter.Value = value;
        }

        public override Guid Parse(object value)
        {
            if (value is byte[])
            {
                return new Guid(value as byte[]);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}