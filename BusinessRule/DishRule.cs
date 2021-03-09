using Common.Data;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFramework;

namespace BusinessRule
{
    public class DishRule
    {
        public int InsertUpdateDish(DishData Dish)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlConn = new SqlConnection(SystemConfigurations.EateryConnectionString);
                SqlConn.Open();
                SqlTran = SqlConn.BeginTransaction();
                int Result = new DishDB().InsertUpdateDish(Dish, SqlTran);
                SqlTran.Commit();
                SqlConn.Close();
                return Result;
            }
            catch (Exception)
            {
                SqlTran.Rollback();
                SqlConn.Close();
                throw;
            }
        }
        public int DeleteDishes(string Ids)
        {
            SqlConnection SqlConn = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlConn = new SqlConnection(SystemConfigurations.EateryConnectionString);
                SqlTran = SqlConn.BeginTransaction();
                int Result = new DishDB().DeleteDishes(Ids, SqlTran);
                SqlTran.Commit();
                SqlConn.Close();
                return Result;
            }
            catch (Exception)
            {
                SqlTran.Rollback();
                SqlConn.Close();
                throw;
            }
        }
    }
}
