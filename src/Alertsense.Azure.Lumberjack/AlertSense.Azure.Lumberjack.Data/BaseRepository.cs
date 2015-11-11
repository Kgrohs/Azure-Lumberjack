using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertSense.Azure.Lumberjack.Contracts.Repositories;
using ServiceStack.Data;

namespace AlertSense.Azure.Lumberjack.Data
{
    public abstract class BaseRepository : IRepositoryBase 
    {
        public IDbConnectionFactory DbFactory { get; set; }
        private IDbConnection _dbConnection;
        public IDbConnection DbConnection
        {
            get
            {
                if (_dbConnection != null && _dbConnection.State == ConnectionState.Closed)
                    _dbConnection.Open();

                _dbConnection = _dbConnection ?? DbFactory.OpenDbConnection();

                return _dbConnection;
            }
            set { _dbConnection = value; }
        }
    }
}
