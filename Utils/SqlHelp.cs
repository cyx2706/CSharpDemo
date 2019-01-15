using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace MvcDemo.Utils
{
    public class SqlHelp
    {
        private string connectionString;

        //构造时先初始化连接字符串
        public SqlHelp()
        {
            connectionString = string.Format(
                 "Data Source={0};Pooling=False;Max Pool Size = 1024;Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
                ConfigurationSettings.AppSettings["SqlServerName"],
                ConfigurationSettings.AppSettings["SqlServerDb"],
                ConfigurationSettings.AppSettings["SqlServerUid"],
                ConfigurationSettings.AppSettings["SqlServerPwd"]
                );
        }

        public string getConnectionString()
        {
            return this.connectionString;
        }

        public DataTable getTable(string sql, Dictionary<String,Object> sqlParams)
        {
            return this.getSet(sql,"tmp",sqlParams).Tables["tmp"];
        }

        public DataSet getSet(string strSql, string table, Dictionary<String, Object> sqlParams)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(strSql, conn))
                {
                    
                    foreach (var sqlParam in sqlParams)
                    {
                        adapter.SelectCommand.Parameters.AddWithValue(sqlParam.Key, sqlParam.Value);
                    }
                    DataSet ds = new DataSet();
                    // 将数据填充到DataSet对象的"用户表"索引对应的内存空间处
                    adapter.Fill(ds, table); //直到这一步才真正连接数据库
                    return ds;
                }
            }
        }



    }
}