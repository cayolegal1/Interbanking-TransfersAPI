using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data
{
    public class PostgreSQLConfiguration
    {
        //public string Connect(string connectionString) => ConnectionString = connectionString;

        public PostgreSQLConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }
    }
}
