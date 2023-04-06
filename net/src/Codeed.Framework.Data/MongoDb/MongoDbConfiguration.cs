using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codeed.Framework.Data.MongoDb
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; } = "";

        public string Database { get; set; } = "";
    }
}
