using BusinessRule;
using Common.Data;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessFacade
{
    public class DishSystem
    {
        public List<DishData> GetDishList()
        {
            try
            {
                return new DishDB().GetDishList();
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
                return new DishDB().GetDishById(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int InsertUpdateDish(DishData Dish)
        {
            try
            {
                return new DishRule().InsertUpdateDish(Dish);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int DeleteDishes(string Ids)
        {
            try
            {
                return new DishRule().DeleteDishes(Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
