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
    public class ContentTreeService : IContentTreeService
    {
        #region Public Constructors

        public ContentTreeService(IEventService eventService)
        {
            _eventService = eventService;
        }

        #endregion

        #region Public Methods

        public Guid CreateNode<T>(Guid? parent, string alias) where T : ItemBase
        {
            var id = Guid.NewGuid();

            var node = new Node { Id = Guid.Empty, Parent = parent, Alias = alias, Type = typeof(T).FullName };

            _eventService.Emit(new BeforeNodeCreated { Data = node });

            const string ResourceName = "Sql/CreateNode.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            var type = TypeUtil.TypeName<T>();

            using (var connection = GetConnection())
            {
                connection.Execute(sql, new { id, parent, alias, type });
            }
            _eventService.Emit(new AfterNodeCreated { Data = node });

            return id;
        }

        public T GetActiveNodeVersion<T>(Guid id)
            where T : ItemBase
        {
            const string ResourceName = "Sql/GetActiveNodeVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id });

                return data.Select(ent => Deserialise<T>(ent.Data)).First();
            }
        }

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            const string ResourceName = "Sql/GetAllNodeVersions.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<NpgsqlNodeVersion>(sql, new { id });
            }
        }

        public Node GetChild(Guid? parent, string alias)
        {
            const string ResourceName = "Sql/GetChild.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<Node>(sql, new { parent, alias }).First();
            }
        }

        public IEnumerable<Node> GetChildren(Guid? id)
        {
            const string ResourceName = "Sql/GetChildren.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<Node>(sql, new { id });
            }
        }

        public Node GetNode(Guid id)
        {
            const string ResourceName = "Sql/GetNode.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                return connection.Query<Node>(sql, new { id }).Single();
            }
        }

        public T GetNodeVersion<T>(Guid id, Guid version)
            where T : ItemBase
        {
            const string ResourceName = "Sql/GetNodeVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id, version });

                return data.Select(ent => Deserialise<T>(ent.Data)).First();
            }
        }

        public object GetNodeVersion(Guid id, Guid version, Type type)
        {
            const string ResourceName = "Sql/GetNodeVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id, version });

                return data.Select(ent => Deserialise(ent.Data, type)).First();
            }
        }

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
        {
            const string ResourceName = "Sql/GetNodeVersion.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);

            using (var connection = GetConnection())
            {
                var data = connection.Query<NpgsqlNodeVersion>(sql, new { id, version });

                return data.Select(ent => DeserialiseAnonymous(ent.Data)).First();
            }
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            throw new NotImplementedException();
        }

        public NodeVersion Save<T>(Guid id, Guid node, T item) where T : ItemBase
        {
            const string ResourceName = "Sql/Save.sql";

            var sql = ResourceUtil.Get<ContentTreeService>(ResourceName);
            var data = Serialise(item);

            _eventService.Emit(new BeforeItemSaved { Data = item });

            using (var connection = GetConnection())
            {
                var ret = connection.Query<NodeVersion>(sql, new { id, node, data }).FirstOrDefault();

                _eventService.Emit(new AfterItemSaved { Data = item });

                return ret;
            }
        }

        public void SetActiveNodeVersion(Guid id, Guid version)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(Environment.GetEnvironmentVariable("DB"));
        }

        #endregion

        #region Private Fields

        IEventService _eventService;

        #endregion
    }
}