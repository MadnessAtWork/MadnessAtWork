using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegsCookieDatabase
{
    public partial class MessagePopup : Form
    {
        public MessagePopup(string message)
        {
            //A form that contains a string message and a button to close the form.
            //This is used to display error messages and successful completion notifications
            InitializeComponent();
            label1.Text = message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
