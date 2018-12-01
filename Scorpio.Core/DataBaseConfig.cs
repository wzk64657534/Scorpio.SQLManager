using Microsoft.Extensions.Options;
using Scorpio.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Scorpio.Core
{
    public class DataBaseConfig
    {
        #region SqlServer链接配置
        /// <summary>
        /// 默认的Sql Server的链接字符串
        /// </summary>
        private static string DefaultSqlConnectionString =@"Data Source=139.196.230.94;Initial Catalog=School;User ID=sa;Password=Ings111...;";
        //private static SQLContact sQLContact = null;
        //public DataBaseConfig(IOptions<SQLContact> setting)
        //{
        //    sQLContact = setting.Value;
        //}
        public static IDbConnection GetSqlConnection(string sqlConnectionString = null)
        {
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                sqlConnectionString = DefaultSqlConnectionString;
            }
            IDbConnection conn = new SqlConnection(sqlConnectionString);
            conn.Open();
            return conn;
        }
        #endregion
    }
}
