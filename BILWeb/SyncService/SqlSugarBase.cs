using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BILWeb.SyncService
{
    public class SqlSugarBase
    {
        public static  SqlSugarClient GetInstance()
        {
            //SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = "Data Source=192.168.250.71; Initial Catalog = WMSDB; Persist Security Info = True; User ID = sa; Password = GPGsec2020; Persist Security Info = True;", DbType = DbType.SqlServer, IsAutoCloseConnection = true });// "Data Source=192.168.100.86;Initial Catalog=ABH_SCG;Persist Security Info=True;User ID=sa;Password=chinetek;Persist Security Info=True;"
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = "Data Source=192.168.250.37;Initial Catalog=WMSDB;Persist Security Info=True;User ID=sa;Password=chinetek;Persist Security Info=True;", DbType = DbType.SqlServer, IsAutoCloseConnection = true });// "Data Source=192.168.100.86;Initial Catalog=ABH_SCG;Persist Security Info=True;User ID=sa;Password=chinetek;Persist Security Info=True;"
            return db;
        }
    }
}
