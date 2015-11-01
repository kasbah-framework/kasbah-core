using System;

namespace Kasbah.Core.ContentTree.Models
{
    public class Node
    {
        #region Public Properties

        public string Alias { get; set; }
        public Guid? CurrentVersionId { get; set; }
        public bool HasChildren{ get; set; }
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        #endregion
    }
}