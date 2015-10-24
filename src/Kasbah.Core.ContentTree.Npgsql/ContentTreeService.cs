using System;
using Kasbah.Core.Models;
using Npgsql;

namespace Kasbah.Core.ContentTree.Npgsql
{
    public class ContentTreeService : IContentTreeService
    {
        NpgsqlConnection GetConnection()
        {
            return null;
        }

        public Tuple<T, DateTime> GetAllItemVersions<T>(Guid id)
        {
            using (var connection = GetConnection())
            {
                throw new NotImplementedException();
            }
        }

        public T GetMostRecentlyCreatedItemVersion<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(Guid id, T item) where T : ItemBase
        {
            throw new NotImplementedException();
        }
    }
}