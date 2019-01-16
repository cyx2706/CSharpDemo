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
using MvcDemo.Exception;

namespace MvcDemo.Utils
{
    public class SqlHelp
    {
        private string connectionString;

        private string cmdTxt;//编译后的sql语句字符串

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
                // sql注入检验
                foreach (var sqlParam in sqlParams)
                {
                    // 如果出现类似sql注入字符串,则抛出错误 SqlHelpException 由dao层根据业务情况处理该错误
                    if (!SqlChecker.IsSafe(sqlParam.Value.ToString()))
                    {
                        throw new SqlHelpException("不安全的字符串(可能造成sql注入问题): " + sqlParam.Value.ToString());
                    }
                    // 替换变量
                    strSql = strSql.Replace(sqlParam.Key.ToString(), sqlParam.Value.ToString());
                }
                // 记录替换变量后的sql语句
                cmdTxt = strSql;
                // 查询数据库
                using (SqlDataAdapter adapter = new SqlDataAdapter(strSql, conn))
                {

                    DataSet ds = new DataSet();
                    // 将数据填充到DataSet对象的"用户表"索引对应的内存空间处
                    adapter.Fill(ds, table); //直到这一步才真正连接数据库
                    return ds;
                }
            }
        }

        public string getCmdTxt()
        {
            return cmdTxt;
        }

    }


    /// <summary>
    /// 防SQL注入检查器
    /// </summary>
    public class SqlChecker
    {

        //Sql注入时,可能出现的sql关键字,可根据自己的实际情况进行初始化,每个关键字由'|'分隔开来
        //private const string StrKeyWord = @"select|insert|delete|from|count(|drop table|update|truncate|asc(|mid(|char(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and";
        private  const string StrKeyWord = @"select|insert|delete|from|drop table|update|truncate|exec master|netlocalgroup administrators|:|net user|or|and";
        //Sql注入时,可能出现的特殊符号,,可根据自己的实际情况进行初始化,每个符号由'|'分隔开来
        //private const string StrRegex = @"-|;|,|/|(|)|[|]|}|{|%|@|*|!|'";
        private const string StrRegex = @"-|=|!|'";

        /// <summary>
        /// 检查_sword是否包涵SQL关键字
        /// </summary>
        /// <param name="_sWord">需要检查的字符串</param>
        /// <returns>存在SQL注入关键字时返回 false，否则返回 true</returns>
        public static bool IsSafe(string _sWord)
        {
            bool result = true;
            //模式1 : 对应Sql注入的可能关键字
            string[] patten1 = StrKeyWord.Split('|');
            //模式2 : 对应Sql注入的可能特殊符号
            string[] patten2 = StrRegex.Split('|');
            //开始检查 模式1:Sql注入的可能关键字 的注入情况
            foreach (string sqlKey in patten1)
            {
                if (_sWord.IndexOf(" " + sqlKey) >= 0 || _sWord.IndexOf(sqlKey + " ") >= 0)
                {
                    //只要存在一个可能出现Sql注入的参数,则直接退出
                    result = false;
                    break;
                }
            }
            //开始检查 模式1:Sql注入的可能特殊符号 的注入情况
            foreach (string sqlKey in patten2)
            {
                if (_sWord.IndexOf(sqlKey) >= 0)
                {
                    //只要存在一个可能出现Sql注入的参数,则直接退出
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}