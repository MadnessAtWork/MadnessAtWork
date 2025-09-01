using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MegsCookieDatabase 
{ 
    //A simple class for handling the transfer of screens. 
     
    internal class ScreenManager
    {
        static public void OpenHome(Form form)
        {
            form.Close();
        }
        static public void OpenHistory()
        {
            HistoryScreen history = new HistoryScreen();
            history.ShowDialog();
            history.Dispose();
        }
        static public void OpenGraph()
        {
            GraphScreen graph = new GraphScreen();
            graph.ShowDialog();
            graph.Dispose();
        }
        static public void OpenNewOrder()
        {
            NewOrderScreen newOrder = new NewOrderScreen();
            newOrder.ShowDialog();
            newOrder.Dispose();
        }
        static public void OpenPopup(string errorMessage)
        {
            MessagePopup error = new MessagePopup(errorMessage);
            error.ShowDialog();
            error.Dispose();
        }
    }
}
