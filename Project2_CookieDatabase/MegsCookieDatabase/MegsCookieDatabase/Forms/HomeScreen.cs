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
    public partial class HomeScreen : Form
    {
        //A simple form that serves as the main menu of the application.
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void HomeScreen_Load(object sender, EventArgs e)
        {

        }
        private void historyButton_Click(object sender, EventArgs e)
        {
            ScreenManager.OpenHistory();
        }
        private void newOrderButton_Click(object sender, EventArgs e)
        {
            ScreenManager.OpenNewOrder();
        }
        private void graphButton_Click(object sender, EventArgs e)
        {
            ScreenManager.OpenGraph();
        }
    }
}
