using System.Collections.Generic;

namespace Eqi.Core.DataAccess.Config
{
    public class DataAccessConfig
    {
        public string SystemName { get; set; }

        public string DatabaseConfig { get; set; }

        public List<string> DataCommandConfig { get; set; }
    }
}
