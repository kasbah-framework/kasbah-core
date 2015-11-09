using System;
using System.Collections.Generic;
using Kasbah.Core.ContentTree.Events;
using Kasbah.Core.ContentTree.Models;
using Kasbah.Core.Events;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
    public class ContentTreeService
    {
        public ContentTreeService(IContentTreeProvider contentTreeProvider, IEventService eventService)
        {
            _contentTreeProvider = contentTreeProvider;
            _eventService = eventService;
        }

        public Guid CreateNode<T>(Guid? parent, string alias) where T : ItemBase
            => CreateNode(parent, alias, typeof(T));

        public Guid CreateNode(Guid? parent, string alias, Type type)
            => CreateNode(parent, alias, type.AssemblyQualifiedName);

        public Guid CreateNode(Guid? parent, string alias, string type)
        {
            var id = Guid.NewGuid();

            var node = new Node { Id = id, Parent = parent, Alias = alias, Type = type };

            _eventService.Emit(new BeforeNodeCreated { Data = node });

            _contentTreeProvider.CreateNode(id, parent, alias, type);

            _eventService.Emit(new AfterNodeCreated { Data = node });

            return id;
        }

        public T GetActiveNodeVersion<T>(Guid id) where T : ItemBase
        {
            return _contentTreeProvider.GetActiveNodeVersion<T>(id);
        }

        public IEnumerable<NodeVersion> GetAllNodeVersions(Guid id)
        {
            return _contentTreeProvider.GetAllNodeVersions(id);
        }

        public Node GetChild(Guid? parent, string alias)
        {
            return _contentTreeProvider.GetChild(parent, alias);
        }

        public IEnumerable<Node> GetChildren(Guid? id)
        {
            return _contentTreeProvider.GetChildren(id);
        }

        public Node GetNode(Guid id)
        {
            return _contentTreeProvider.GetNode(id);
        }

        public T GetNodeVersion<T>(Guid id, Guid version) where T : ItemBase
        {
            return GetNodeVersion(id, version) as T;
        }

        public object GetNodeVersion(Guid id, Guid version, Type type)
        {
            return _contentTreeProvider.GetNodeVersion(id, version, type);
        }

        public IDictionary<string, object> GetNodeVersion(Guid id, Guid version)
        {
            return _contentTreeProvider.GetNodeVersion(id, version);
        }

        public void MoveNode(Guid id, Guid? parent)
        {
            var node = new Node { Id = id, Parent = parent };

            _eventService.Emit(new BeforeNodeMoved { Data = node });

            _contentTreeProvider.MoveNode(id, parent);

            _eventService.Emit(new AfterNodeMoved { Data = node });
        }

        public NodeVersion Save<T>(Guid id, Guid node, T item) where T : ItemBase
        {
            return Save(id, node, (object)item);
        }

        public NodeVersion Save(Guid id, Guid node, object item)
        {
            _eventService.Emit(new BeforeItemSaved { Data = item as ItemBase, Node = node });

            var ret =  _contentTreeProvider.Save(id, node, item);
            
            _eventService.Emit(new AfterItemSaved { Data = item as ItemBase, Node = node });

            return ret;
        }

        public void SetActiveNodeVersion(Guid id, Guid version)
        {
            _contentTreeProvider.SetActiveNodeVersion(id, version);

            _eventService.Emit<NodeActiveVersionSet>(new NodeActiveVersionSet
            {
                Data = new Node
                {
                    Id = id,
                    ActiveVersionId = version
                }
            });
        }

        readonly IContentTreeProvider _contentTreeProvider;
        readonly IEventService _eventService;
    }
}