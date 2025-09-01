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
    public partial class GraphScreen : Form
    {
        //A form that displays pie charts of cookie and mixin tallies, allowing for visual representation of data.
        private DataSet ds;
        public GraphScreen()
        {
            // Retrieves tallies from the database and initializes the form components.
            ds = DataBaseHandler.GatherTallies();
            InitializeComponent();
            chart1.DataSource = ds.Tables["CookieTally"];
            chart2.DataSource = ds.Tables["MixinTally"];

            //Constructs the pie charts for cookie and mixin tallies, setting properties such as chart type, axis types, and labels.
            var cookieSeries = new System.Windows.Forms.DataVisualization.Charting.Series("CookieTally");
            cookieSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            cookieSeries.XValueMember = "CookieType";
            cookieSeries.YValueMembers = "cTally";
            cookieSeries.IsValueShownAsLabel = true;
            cookieSeries["DrawingStyle"] = "Cylinder";
            cookieSeries.Points.Clear();
            cookieSeries.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
            cookieSeries.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
            chart1.Series.Add(cookieSeries);
            chart1.Titles.Add("Cookie Totals");

            var mixinSeries = new System.Windows.Forms.DataVisualization.Charting.Series("MixinTally");
            mixinSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            mixinSeries.XValueMember = "MixinType";
            mixinSeries.YValueMembers = "mTally";
            mixinSeries.IsValueShownAsLabel = true;
            cookieSeries["DrawingStyle"] = "Cylinder";
            mixinSeries.Points.Clear();
            mixinSeries.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
            mixinSeries.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Primary;
            chart2.Series.Add(mixinSeries);
            chart2.Titles.Add("Mixin Totals");
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void GraphScreen_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
