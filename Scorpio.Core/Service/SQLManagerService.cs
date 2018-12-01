using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Linq;
using Scorpio.Core.Model;

namespace Scorpio.Core.Service
{
    public class SQLManagerService
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        public static List<Table> GetSQLTableList()
        {
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection(null))
            {
                string querySql = @"select ROW_NUMBER() OVER (ORDER BY a.name) AS Num, 
a.name AS TableName,
CONVERT(NVARCHAR(100),isnull(g.[value],'')) AS TableExplain
from
sys.tables a left join sys.extended_properties g
on (a.object_id = g.major_id AND g.minor_id = 0)";
                return conn.Query<Table>(querySql).ToList();
            }
        }

        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<TableStructure> GetTableStructure(string tableName)
        {
            string sql = $@"SELECT (case when a.colorder=1 then d.name else null end) TableName, 
                            a.colorder Num,a.name FieldName,
                            (case when (SELECT count(*) FROM sysobjects 
                            WHERE (name in (SELECT name FROM sysindexes 
                            WHERE (id = a.id) AND (indid in 
                            (SELECT indid FROM sysindexkeys 
                            WHERE (id = a.id) AND (colid in 
                            (SELECT colid FROM syscolumns WHERE (id = a.id) AND (name = a.name))))))) 
                            AND (xtype = 'PK'))>0 then 1 else 0 end) IsKey,b.name FieldType,
                            COLUMNPROPERTY(a.id,a.name,'PRECISION') as FieldLength, 
                            isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as DecimalDigit,(case when a.isnullable=1 then 1 else 0 end) [IsNull], 
                            isnull(e.text,'')  DefaultValue,isnull(g.[value], ' ') AS Explain
                            FROM syscolumns a 
                            left join systypes b on a.xtype=b.xusertype 
                            inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties' 
                            left join syscomments e on a.cdefault=e.id 
                            left join sys.extended_properties g on a.id=g.major_id AND a.colid=g.minor_id
                            left join sys.extended_properties f on d.id=f.class and f.minor_id=0
                            where b.name is not null
                            and d.name='{tableName}' --如果只查询指定表,加上此条件
                            order by a.id,a.colorder";
            using (IDbConnection conn = DataBaseConfig.GetSqlConnection(null))
            {
                return conn.Query<TableStructure>(sql).ToList();
            }
        }

        public static int UpdateTableFieldExplain(string table, string column, string value)
        {
            try
            {
                string sql = $"execute sp_updateextendedproperty 'MS_Description','{value}','user','dbo','table','{table}','column','{column}'";

                using (IDbConnection conn = DataBaseConfig.GetSqlConnection(null))
                {
                    conn.Execute(sql);
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int AddTableFieldExplain(string table, string column, string value)
        {
            try
            {
                string sql = $"execute sp_addextendedproperty 'MS_Description','{value}','user','dbo','table','{table}','column','{column}'";
                using (IDbConnection conn = DataBaseConfig.GetSqlConnection(null))
                {
                     conn.Execute(sql);
                }
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 新增表说明
        /// </summary>
        /// <param name="table"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int AddTableExplain(string table, string value)
        {
            try
            {
                string sql = $"execute sp_addextendedproperty 'MS_Description','{value}','user','dbo','table','{table}',null,null";
                using (IDbConnection conn = DataBaseConfig.GetSqlConnection(null))
                {
                    conn.Execute(sql);
                }
                return 1;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 更新表说明
        /// </summary>
        /// <param name="table"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int UpdateTableExplain(string table, string value)
        {
            try
            {
                string sql = $"execute sp_updateextendedproperty 'MS_Description','{value}','user','dbo','table','{table}',null,null";
                using (IDbConnection conn = DataBaseConfig.GetSqlConnection(null))
                {
                    conn.Execute(sql);
                }
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }
    }
}
