using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class DishData
    {
        private int _dishId;
        public int DishId
        {
            get { return _dishId; }
            set { _dishId = value; }
        }
        private string _dishName;
        public string DishName
        {
            get { return _dishName; }
            set { _dishName = value; }
        }
        private string _dishType;
        public string DishType
        {
            get { return _dishType; }
            set { _dishType = value; }
        }
        private int _dishPrice;
        public int DishPrice
        {
            get { return _dishPrice; }
            set { _dishPrice = value; }
        }
    }
}
