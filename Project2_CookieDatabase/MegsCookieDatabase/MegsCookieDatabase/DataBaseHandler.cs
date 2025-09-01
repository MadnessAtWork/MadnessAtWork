using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MegsCookieDatabase
{
    internal class DataBaseHandler
    // This class handles all database interactions for the Meg's Cookie Database application.
    {
        // The connection string to the local database.
        static private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\dwcar\\source\\repos\\MegsCookieDatabase\\CookieDatabase.mdf;Integrated Security=True";
        static public List<Order> GetAllOrders()
        {
            //Collects all orders from the database and returns them as a list of Order objects.
            const string GetOrdersQuery = "select * from Orders;";
            var orders = new List<Order>();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetOrdersQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    //Reads each row from the Orders table and creates an Order object.
                                    int orderID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                    string phone = reader.IsDBNull(1) ? null : reader.GetString(1);
                                    string name = reader.IsDBNull(2) ? null : reader.GetString(2);
                                    DateTime date = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);
                                    decimal price = reader.IsDBNull(4) ? 0m : reader.GetDecimal(4);
                                    List<Cookies> cookies = new List<Cookies>();
                                    List<Mixins> mixins = new List<Mixins>();
                                    //This numeric range is based on the number of cookie types available, this number is stored publicly under Program
                                    for (int i = 0; i < Program.cookieNum; i++)
                                    {
                                        if (!reader.IsDBNull(i + 5) && reader.GetBoolean(i + 5))
                                        {
                                            cookies.Add((Cookies)i);
                                        }
                                    }
                                    for (int i = 0; i < Program.mixInNum; i++)
                                    {
                                        if (!reader.IsDBNull(i + 5 + Program.cookieNum) && reader.GetBoolean(i + 5 + Program.cookieNum))
                                        {
                                            mixins.Add((Mixins)i);
                                        }
                                    }
                                    orders.Add(new Order(phone, orderID, name, price, date, cookies, mixins));
                                }
                            }
                        }
                    }
                }
                return orders;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
            }
            return null;
        }

        static public DataSet GatherTallies()
        {
            //Extracts the number of times each cookie and mixin are ordered
            //from the database and returns it as a dataset to be used in the chart control.
            const string GetOrdersQuery = "select * from Orders;";

            //Create a dataset to hold the tally information
            DataSet ds = new DataSet();
            DataTable tCook = new DataTable("CookieTally");
            DataTable tMix = new DataTable("MixinTally");
            //create columns for the cookies
            DataColumn cType = new DataColumn("CookieType");
            DataColumn cNum = new DataColumn("cTally", typeof(int));
            tCook.Columns.Add(cType);
            tCook.Columns.Add(cNum);
            //create columns for the mixins
            DataColumn mType = new DataColumn("MixinType");
            DataColumn mNum = new DataColumn("mTally", typeof(int));
            tMix.Columns.Add(mType);
            tMix.Columns.Add(mNum);
            //add all the tables to the dataset
            ds.Tables.Add(tCook);
            ds.Tables.Add(tMix);

            //Creates two collections and populates them with the cookie and mixin types, setting their initial counts to 0.
            Dictionary<string, int> cookieTally = new Dictionary<string, int>();
            Dictionary<string, int> mixinTally = new Dictionary<string, int>();
            foreach (Cookies cookie in Program.allCookies)
            {
                cookieTally.Add(cookie.ToString(), 0);
            }
            foreach (Mixins mixin in Program.allMixins)
            {
                mixinTally.Add(mixin.ToString(), 0);
            }
            try //Connects to the database
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetOrdersQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    //This numeric range is based on the number of cookie types available, this number is stored publicly under Program
                                    for (int i = 0; i < Program.cookieNum; i++)
                                    {
                                        //Runs through each cookies status for each order and increments the appropriate tally
                                        if (reader.GetBoolean(i + 5))
                                        {
                                            Cookies curCookie = (Cookies)i;
                                            cookieTally[curCookie.ToString()] += 1;
                                        }
                                    }
                                    for (int i = 0; i < Program.mixInNum; i++)
                                    {
                                        //runs through each mixin's status for each order and increments the appropriate tally
                                        if (reader.GetBoolean(i + 5 + Program.cookieNum))
                                        {
                                            Mixins curMixin = (Mixins)i;
                                            mixinTally[curMixin.ToString()] += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Inputs the tally data into the dataset tables
                DataRow dataRow;
                foreach (var mixin in mixinTally)
                {
                    dataRow = tMix.NewRow();
                    dataRow["MixinType"] = mixin.Key;
                    dataRow["mTally"] = mixin.Value;
                    tMix.Rows.Add(dataRow);
                }
                foreach (var cookie in cookieTally)
                {
                    dataRow = tCook.NewRow();
                    dataRow["CookieType"] = cookie.Key;
                    dataRow["cTally"] = cookie.Value;
                    tCook.Rows.Add(dataRow);
                }
                return ds;
            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
            }
            return null;
        }
        static public bool WriteToDatabase(Order order)
        {
            //The command used in the NewOrderScreen to write an order to the database.

            //This method checks to see if the customer already exists in the database, and if not, adds them.
            if (!CustomerExists(order.phone))
            {
                //if the customer cannot be added, the attempt to write the order will fail.
                if (!AddCustomer(order.phone, order.name))
                {
                    Debug.WriteLine("Failed to add customer to database.");
                    return false;
                }
            }
            //Creates two lists of bools to represent whether a cookie or mixin is included in the order
            List<bool> cookies = new List<bool>();
            List<bool> mixins = new List<bool>();
            for (int i = 0; i < Program.cookieNum; i++)
            {
                if (order.cookies.Contains((Cookies)i))
                {
                    cookies.Add(true);
                }
                else
                {
                    cookies.Add(false);
                }
            }
            for (int i = 0; i < Program.mixInNum; i++)
            {
                if (order.mixins.Contains((Mixins)i))
                {
                    mixins.Add(true);
                }
                else
                {
                    mixins.Add(false);
                }
            }

            //Starts the SQL command to insert the order into the database, leaving the input as parameters to prevent SQL injection and leaves the
            //end blank so that the statuses of the cookies and mixins can be appended.
            string insertOrder = "insert into Orders (customer_phone, customer_name, order_date, order_price, bbcc_cookie, cs_cookie, od_cookie, " +
                "cb_cookie, cc_cookie, cgsb_cookie, pbp_cookie, bwt_cookie, " +
                "dcc_mixin, wcc_mixin, mcc_mixin, sscc_mixin, bcc_mixin, raisin_mixin, pretzel_mixin, sprinkle_mixin, mm_mixin, oreo_mixin)" +
                " values (@Phone, @Name, @DateParam, @Price";

            //The status of each cookie and mixin is appended to the SQL command.
            for (int i = 0; i < cookies.Count; i++)
            {
                insertOrder += $", @Cookie{i}";
            }
            for (int i = 0; i < mixins.Count; i++)
            {
                insertOrder += $", @Mixin{i}";
            }
            insertOrder += ");";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertOrder, conn))
                        {
                            //The actual parameters are added to the command, including the phone number, name, date, price, and the statuses of the cookies and mixins.
                            cmd.Parameters.AddWithValue("@Phone", order.phone ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Name", order.name ?? (object)DBNull.Value);
                            cmd.Parameters.Add(new SqlParameter("@DateParam", SqlDbType.Date) { Value = order.orderDate.Date });
                            cmd.Parameters.AddWithValue("@Price", order.price);

                            for (int i = 0; i < cookies.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@Cookie{i}", cookies[i]);
                            }
                            for (int i = 0; i < mixins.Count; i++)
                            {
                                cmd.Parameters.AddWithValue($"@Mixin{i}", mixins[i]);
                            }

                            //The command is then executed to insert the order into the database.
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return true; // Order written successfully
            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
                return false; // Failed to write to database
            }
        }
        private static bool CustomerExists(string phone)
        {
            //this method checks to determine if a customer already exists in the database.
            const string GetCustomersQuery = "select customer_phone from Customers;";
            var orders = new List<Order>();
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = GetCustomersQuery;
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (!reader.IsDBNull(0))
                                    {
                                        string curPhone = reader.GetString(0);
                                        if (curPhone == phone)
                                        {
                                            return true; // Customer exists
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
            }
            return false;
        }
        private static bool AddCustomer(string phone, string name)
        {
            string insertOrder = "insert into Customers values (@Phone, @Name, @Date);";
            //insert a new customer into the database
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (SqlCommand cmd = new SqlCommand(insertOrder, conn))
                        {
                            cmd.Parameters.AddWithValue("@Phone", phone ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Name", name ?? (object)DBNull.Value);
                            cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.Date) { Value = DateTime.Today.Date });

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return true; // Customer added successfully
            }
            catch (Exception eSql)
            {
                Debug.WriteLine($"Exception: {eSql.Message}");
                return false; // Failed to add customer
            }
        }
    }
}
