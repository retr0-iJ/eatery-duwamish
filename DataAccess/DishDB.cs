using Common.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFramework;

namespace DataAccess
{
    public class DishDB
    {
        public List<DishData> GetDishList()
        {
            try
            {
                string SpName = "dbo.Dish_Get";
                List<DishData> ListDish = new List<DishData>();
                DishData Dish = new DishData();
                using(SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    using(SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            while (Reader.Read())
                            {
                                Dish = new DishData();
                                Dish.DishId = Convert.ToInt32(Reader["DishID"]);
                                Dish.DishName = Convert.ToString(Reader["Name"]);
                                Dish.DishType = Convert.ToString(Reader["DishType"]);
                                Dish.DishPrice = Convert.ToInt32(Reader["Price"]);
                                ListDish.Add(Dish);
                            }
                        }
                    }
                    SqlConn.Close();
                }
                return ListDish;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DishData GetDishById(int Id)
        {
            try
            {
                string SpName = "dbo.Dish_GetById";
                DishData Dish = null;
                using(SqlConnection SqlConn = new SqlConnection())
                {
                    SqlConn.ConnectionString = SystemConfigurations.EateryConnectionString;
                    SqlConn.Open();
                    SqlCommand SqlCmd = new SqlCommand(SpName, SqlConn);
                    SqlCmd.CommandType = CommandType.StoredProcedure;
                    SqlCmd.Parameters.Add(new SqlParameter("@ID", Id));
                    using(SqlDataReader Reader = SqlCmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            Reader.Read();
                            Dish = new DishData();
                            Dish.DishId = Convert.ToInt32(Reader["DishID"]);
                            Dish.DishName = Convert.ToString(Reader["Name"]);
                            Dish.DishType = Convert.ToString(Reader["DishType"]);
                            Dish.DishPrice = Convert.ToInt32(Reader["Price"]);
                        }
                    }
                    SqlConn.Close();
                }
                return Dish;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int InsertUpdateDish(DishData Dish, SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.Dish_InsertUpdate";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add(new SqlParameter("@DishID", Dish.DishId));
                SqlCmd.Parameters.Add(new SqlParameter("@DishName", Dish.DishName));
                SqlCmd.Parameters.Add(new SqlParameter("@DishType", Dish.DishType));
                SqlCmd.Parameters.Add(new SqlParameter("@Price", Dish.DishPrice));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int DeleteDishes(string Ids,SqlTransaction SqlTran)
        {
            try
            {
                string SpName = "dbo.Dish_Delete";
                SqlCommand SqlCmd = new SqlCommand(SpName, SqlTran.Connection, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add(new SqlParameter("@DishIDs", Ids));
                return SqlCmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
