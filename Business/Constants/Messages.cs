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
        //BusinessRules
        public static string BusinessRulesNotComply = "This action does not comply with BusinessRules.";
    }
}
