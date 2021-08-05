using Dapper.CRUD.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Dapper.CRUD.DAL.Data
{
    public class SqlDatabaseStrategy : IDatabaseStrategy
    {
        private DbConnection _connection;
        public DbConnection Connection
        {
            get
            {
                if(_connection == null )//|| _connection=="")
                {
                    _connection = new SqlConnection(@"data source=DESKTOP-GOOERDL\SQLEXPRESS;initial catalog=Dapper20210804;persist security info=True;Integrated Security=SSPI;");
                    _connection.Open();
                }
                else if(_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return _connection;
            }
        }

        public void SeedDatabase()
        {
            //throw new NotImplementedException();
        }
    }
}
