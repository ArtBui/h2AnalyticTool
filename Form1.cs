using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace h2AnalyticTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            chart1.Titles.Add("Manometer 1");
            chart1.Titles["Title1"].Text = "Bar";
            chart1.Titles["Title2"].Text = "Celsius";



            chart1.Series["Pressure"].Points.AddXY("17:54:23.309", "30.123");
            chart1.Series["Pressure"].Points.AddXY("17:54:23.879", "35.534");
            chart1.Series["Pressure"].Points.AddXY("17:54:24.256", "37.355");
            chart1.Series["Pressure"].Points.AddXY("17:54:24.459", "40.262");
            chart1.Series["Pressure"].Points.AddXY("17:54:24.945", "47.794");
            chart1.Series["Pressure"].Points.AddXY("17:54:25.325", "55.262");

            chart1.Series["Temperature"].Points.AddXY("17:54:23.309", "-20.342");
            chart1.Series["Temperature"].Points.AddXY("17:54:23.879", "-20.493");
            chart1.Series["Temperature"].Points.AddXY("17:54:24.256", "-20.567");
            chart1.Series["Temperature"].Points.AddXY("17:54:24.459", "-20.790");
            chart1.Series["Temperature"].Points.AddXY("17:54:24.945", "-20.804");
            chart1.Series["Temperature"].Points.AddXY("17:54:25.325", "-20.948");

            chart2.Titles["Title1"].Text = "Flow 1";
            chart2.Titles["Title2"].Text = "Liters";

            chart2.Series["Flow"].Points.AddXY("17:54:23.309", "3.123");
            chart2.Series["Flow"].Points.AddXY("17:54:23.879", "3.534");
            chart2.Series["Flow"].Points.AddXY("17:54:24.256", "3.355");
            chart2.Series["Flow"].Points.AddXY("17:54:24.459", "4.262");
            chart2.Series["Flow"].Points.AddXY("17:54:24.945", "4.794");
            chart2.Series["Flow"].Points.AddXY("17:54:25.325", "5.262");

            

        }
    }
}
