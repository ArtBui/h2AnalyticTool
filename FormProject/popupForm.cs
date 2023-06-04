using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace h2AnalyticTool
{
    public partial class popupForm : Form
    {
        Chart sourceChart;
        public popupForm()
        {
            InitializeComponent();
            
        }

        public popupForm(Chart sourceChart)
        {
            InitializeComponent();            
            
            System.IO.MemoryStream myStream = new System.IO.MemoryStream();
            Chart chart2 = new Chart();
            sourceChart.Serializer.Save(myStream);
            chart1.Serializer.Load(myStream);
            chart1.ChartAreas[0].AxisY.Title = sourceChart.ChartAreas[0].AxisY.Title;
            chart1.ChartAreas[0].AxisY2.Enabled = sourceChart.ChartAreas[0].AxisY2.Enabled;
            chart1.ChartAreas[0].AxisY2.Title = sourceChart.ChartAreas[0].AxisY2.Title;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 50;
            chart1.Dock = DockStyle.Fill;

        }

        public popupForm(Chart sourceChart, int snapshot)
        {
            InitializeComponent();
            this.sourceChart = sourceChart;
            foreach (var series in sourceChart.Series)
                {            
                    chart1.Series.Add(series);
                }
            chart1.ChartAreas[0].AxisY.Title = sourceChart.ChartAreas[0].AxisY.Title;
            chart1.ChartAreas[0].AxisY2.Enabled = sourceChart.ChartAreas[0].AxisY2.Enabled;
            chart1.ChartAreas[0].AxisY2.Title = sourceChart.ChartAreas[0].AxisY2.Title;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 50;
            chart1.Dock = DockStyle.Fill;

        }

        private void popupForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
