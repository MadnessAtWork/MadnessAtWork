using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegsCookieDatabase
{
    internal class Order
    {
        //This class is used to represent an order in the database.
        public int orderID { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public DateTime orderDate { get; set; }
        public List<Cookies> cookies;
        public List<Mixins> mixins;
        public string orderDetails
        {
            get
            {
                StringBuilder details = new StringBuilder();
                details.AppendLine("Cookies: " + string.Join(", ", cookies));
                details.AppendLine(". Mixins: " + string.Join(", ", mixins));
                return details.ToString();
            }
        }

        public Order(string phone, int orderID, string name, decimal price, DateTime orderDate, List<Cookies> cookies, List<Mixins> mixins)
        {
            this.phone = phone;
            this.orderID = orderID;
            this.name = name;
            this.price = price;
            this.orderDate = orderDate;
            this.cookies = cookies;
            this.mixins = mixins;
        }

        public Order(string phone, int orderID, string name, decimal price, List<Cookies> cookies, List<Mixins> mixins)
        {
            this.phone = phone;
            this.orderID = orderID;
            this.name = name;
            this.price = price;
            this.cookies = cookies;
            this.mixins = mixins;
            this.orderDate = DateTime.Today;
        }
    }
}
