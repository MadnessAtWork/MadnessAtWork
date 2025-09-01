using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegsCookieDatabase
{
    public partial class HistoryScreen : Form
    {
        // A form that displays the history of orders made, allowing filtering by phone number, date, and selected cookies.

        //A list to hold all orders retrieved from the database, it is not altered once the form is opened.
        private readonly List<Order> fullOrders = new List<Order>();
        //A list to hold the filtered orders based on user input, this is altered as the user interacts with the form,
        //resetting to the fullOrders when the status changes
        private List<Order> filteredOrders = new List<Order>();
        //An AutoCompleteStringCollection to hold unique phone numbers for the phone number text box's autocomplete feature.
        private AutoCompleteStringCollection phoneNums = new AutoCompleteStringCollection();
        public HistoryScreen()
        {
            //Populates the list with orders from the database and initializes the form components.
            fullOrders = DataBaseHandler.GetAllOrders();
            foreach (Order order in fullOrders)
            {
                if (!phoneNums.Contains(order.phone))
                {
                    phoneNums.Add(order.phone);
                }
            }
            InitializeComponent();
            //Fills the checked list box with all cookies from the Program.allCookies list.
            checkedListBox1.Items.Clear();
            foreach (var cookie in Program.allCookies)
            {
                checkedListBox1.Items.Add(cookie.ToString());
            }

            //Copies the list of orders to the filteredOrders list, which will be displayed in the DataGridView.
            filteredOrders = fullOrders.ToList();
            dataGridView1.DataSource = filteredOrders;
            dataGridView1.Columns["orderDetails"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            //Sets up the AutoComplete feature for the phone number text box using the phoneNums collection,
            //this is to help with the complexity to remembering 10 unique digits
            textBox1.AutoCompleteCustomSource = phoneNums;
            textBox1.AutoCompleteMode = AutoCompleteMode.Append;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void HistoryScreen_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //When the date picker value changes, it filters the orders based on the new date.
            Filter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //when the checked items in the checked list box change, it filters the orders based on the selected cookies.
            Filter();
        }
        private void Filter()
        {
            //Clears all displayed orders from the view to prevent errors when filtering.
            dataGridView1.DataSource = null;
            filteredOrders.Clear();
            foreach (Order order in fullOrders)
            {
                if ((order.phone.Equals(textBox1.Text) || textBox1.Text.Equals("")) && 
                    dateTimePicker1.Value < order.orderDate && dateTimePicker2.Value > order.orderDate)
                {
                    if (checkedListBox1.CheckedItems.Count == 0)
                    {
                        // If no cookies are checked, add the order if it matches the phone and date
                        filteredOrders.Add(order);
                    }
                    else
                    {
                        foreach (var item in checkedListBox1.CheckedItems)
                        {
                            if (order.cookies.Any(cookie => cookie.ToString().Equals(item.ToString())))
                            {
                                filteredOrders.Add(order);
                                break; // No need to check other items if one matches.
                                       // This effectively makes the filtering by cookie type an OR statement.
                            }
                        }
                    }
                }
            }
            dataGridView1.DataSource = filteredOrders;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //when the text in the phone number text box changes, it filters the orders based on the new phone number.
            Filter();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            //when the end date picker value changes, it filters the orders based on the new end date.
            Filter();
        }
    }
}
