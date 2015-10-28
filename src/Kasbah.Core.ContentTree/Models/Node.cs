using System;

namespace Kasbah.Core.ContentTree.Models
{
    public class Node
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Alias { get; set; }

        public Guid? CurrentVersionId { get; set; }
    }
}