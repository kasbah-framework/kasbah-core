using System;

namespace Kasbah.Core.ContentTree.Models
{
    public class NodeVersion
    {
        #region Public Properties

        public string Data { get; set; }
        public Guid Id { get; set; }
        public Guid NodeId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        #endregion
    }
}