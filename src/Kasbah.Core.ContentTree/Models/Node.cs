using System;

namespace Kasbah.Core.ContentTree.Models
{
    public class Node
    {
        #region Public Properties

        public Guid? ActiveVersion { get; set; }
        public string Alias { get; set; }
        public bool HasChildren { get; set; }
        public Guid Id { get; set; }

        public Guid? Parent { get; set; }
        public string Type { get; set; }

        #endregion
    }
}