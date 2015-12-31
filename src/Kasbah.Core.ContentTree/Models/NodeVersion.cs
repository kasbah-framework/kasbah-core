using System;
using System.Collections.Generic;

namespace Kasbah.Core.ContentTree.Models
{
    public class NodeVersion
    {
        #region Public Properties

        public DateTime Created { get; set; }
        public Guid Id { get; set; }
        public DateTime Modified { get; set; }
        public Guid NodeId { get; set; }
        public string Data { get; set; }

        #endregion
    }
}