using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MvcDemo.Utils;
using System.Data.SqlClient;
using MvcDemo.Exception;

namespace MvcDemo.Dao
{
    public class UserDao
    {

        /**
         * @param name 用户名字符串
         * @param pwd 用户密码
         * @descrition 通过
         */
        public DataRowCollection FindUserByNameAndPwd(string name, string pwd)
        {
            SqlHelp sqlHelp = new SqlHelp();
            string sql =
                "SELECT user_id,user_name,user_nick,user_status " +
                "FROM w_user " +
                "WHERE  1=1 " +
                "AND user_name = '@name' " +
                "AND user_pwd = '@pass'";

            
            Dictionary<string, object> sqlParams = new Dictionary<string, object>();
            sqlParams.Add("@name", name);
            sqlParams.Add("@pass", pwd);
            try
            {
                return sqlHelp.getTable(sql, sqlParams).Rows;
            }
            catch(SqlHelpException e)
            {
                throw new LoginException("您的输入存在异常,请重新输入");
            }
            catch(System.Exception e)
            {
                // 记录到日志
                string debug =  sqlHelp.getCmdTxt() + "\n" + e.StackTrace;
                // 抛出业务错误
                throw new LoginException("系统繁忙，请稍后再试");
            }
        }
    }
}