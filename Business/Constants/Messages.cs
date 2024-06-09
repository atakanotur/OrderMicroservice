using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class Messages
    {
        //Order
        public static string OrderCreated = "Order is created!";
        public static string OrderNotCreated = "Order is not created!";
        public static string OrderUpdated = "Order is updated!";
        public static string OrderNotUpdated = "Order is not updated!";
        public static string OrderDeleted = "Order is deleted!";
        public static string OrderNotDeleted = "Orders is not deleted!";
        public static string OrdersListed = "Orders is listed!";
        public static string OrdersNotListed = "Order is not listed!";
        public static string OrderAlreadyExist = "This order already exist!";
        public static string OrderStatusChanged = "Order status changed!";
        public static string OrderStatusNotChanged = "Order status not changed!";
        //Product
        public static string ProductCreated = "Product is created!";
        public static string ProductNotCreated = "Product is not created!";
        public static string ProductUpdated = "Product is updated!";
        public static string ProductNotUpdated = "Product is not updated!";
        public static string ProductDeleted = "Product is deleted!";
        public static string ProductNotDeleted = "Products is not deleted!";
        public static string ProductsListed = "Products is listed!";
        public static string ProductsNotListed = "Product is not listed!";
        public static string ProductAlreadyExist = "This product already exist!";
        public static string ProductStatusChanged = "Product status changed!";
        public static string ProductStatusNotChanged = "Product status not changed!";
        //BusinessRules
        public static string BusinessRulesNotComply = "This action does not comply with BusinessRules.";
    }
}
