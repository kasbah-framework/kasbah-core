using System;

namespace Kasbah.Core.ContentTree.Models
{
    public class NodeVersion
    {
        #region Public Properties

        public string Data { get; set; }
        public Guid Id { get; set; }
        public Guid NodeId { get; set; }
        public DateTime Timestamp { get; set; }

        #endregion
    }
}