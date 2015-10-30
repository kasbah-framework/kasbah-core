using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kasbah.Core.ContentTree.Models;

namespace Kasbah.Core.ContentTree.Npgsql.Models
{
    public class NpgsqlNodeVersion : NodeVersion
    {
        public string Data { get; set; }
    }
}
