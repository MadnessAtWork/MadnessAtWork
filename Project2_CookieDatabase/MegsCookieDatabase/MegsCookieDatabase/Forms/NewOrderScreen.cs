using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegsCookieDatabase
{
    public partial class NewOrderScreen : Form
    {
        
        public NewOrderScreen()
        {
            InitializeComponent();
            checkedListBox1.Items.Clear();
            //creates a list the size of the number of cookies and fills it with the names of the cookies
            string[] cookieNames = new string[Program.cookieNum];
            for (int i = 0; i < cookieNames.Length; i++)
            {
                cookieNames[i] = ((Cookies)i).ToString();
            }
            checkedListBox1.Items.AddRange(cookieNames);
            checkedListBox2.Items.Clear();
            //the same for mixins, creates a list the size of the number of mixins and fills it with the names of the mixins
            string[] mixinNames = new string[Program.mixInNum];
            for (int i = 0; i < mixinNames.Length; i++)
            {
                mixinNames[i] = ((Mixins)i).ToString();
            }
            checkedListBox2.Items.AddRange(mixinNames);
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SubmitOrder();
        }
        public void SubmitOrder() 
        {             
            string phone = textBox1.Text;
            string name = textBox2.Text;
            DateTime date = dateTimePicker1.Value;
            List<Cookies> cookies = new List<Cookies>();
            List<Mixins> mixins = new List<Mixins>();
            //creates a temporary price holder as well as a future price holder to utilize tryParse.
            //If the true price holder isn't changed, it will throw an error when the system checkss
            //whether the price is negative
            string tempPrice = textBox3.Text;
            decimal price = -1;
            //Checks if the phone number is 10 digits long and contains only digits
            if (phone.Length == 10 && Program.AreDigitsOnly(phone))
            {
                if (name.Length > 50) //A simple check to ensure the name is not too long
                {
                    ScreenManager.OpenPopup("The name was too long, please enter a name with less than 50 characters");
                    return;
                }
                //Checks if the price is a valid decimal number and is not negative
                if (decimal.TryParse(tempPrice, out price) && price >= 0)
                {
                    //Populates both lists with the checked items from the checked list boxes
                    foreach (Cookies cookie in Program.allCookies)
                    {
                        if (checkedListBox1.CheckedItems.Contains(cookie.ToString()))
                        {
                            cookies.Add(cookie);
                        }
                    }
                    foreach (Mixins mixin in Program.allMixins)
                    {
                        if (checkedListBox2.CheckedItems.Contains(mixin.ToString()))
                        {
                            mixins.Add(mixin);
                        }
                    }
                    //Attempts to write to the database, returning an error in the event that an issue occurs in the SQL process.
                    if (DataBaseHandler.WriteToDatabase(new Order(phone, -1, name, price, date, cookies, mixins)))
                    {
                        ScreenManager.OpenPopup("Order successfully submitted");
                        ScreenManager.OpenHome(this);
                    }
                    else
                    {
                        ScreenManager.OpenPopup("There was an error submitting the order, please try again later");
                    }
                }
                else
                {
                    ScreenManager.OpenPopup("The price was improperly formatted, please enter an acceptable positive decimal number");
                    return;
                }
            }
            else
            {
                ScreenManager.OpenPopup("The Phone Number was improperly formatted, please enter ten numbers without any other formatting");
                return;
            }
        }

    }
}
