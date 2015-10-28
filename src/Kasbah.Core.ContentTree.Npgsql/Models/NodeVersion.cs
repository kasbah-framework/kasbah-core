using System;

namespace Kasbah.Core.ContentTree.Npgsql.Models
{
    public class NodeVersion
    {
        public Guid Id { get; set; }
        public Guid NodeId { get; set; }
        public DateTime Timestamp { get; set; }
        public byte[] Data { get; set; }
    }
}