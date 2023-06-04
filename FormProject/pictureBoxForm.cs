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
    public partial class pictureBoxForm : Form
    {
        public pictureBoxForm()
        {
            InitializeComponent();
        }

        private void pictureBoxForm_Load(object sender, EventArgs e)
        {

        }

        public void setValves(int value, int[][] valves)
        {
            v1.BackColor = valves[0][value] == 0 ? Color.Red : Color.Green;
            v2.BackColor = valves[1][value] == 0 ? Color.Red : Color.Green;
            v3.BackColor = valves[2][value] == 0 ? Color.Red : Color.Green;
            v4.BackColor = valves[3][value] == 0 ? Color.Red : Color.Green;
            v5.BackColor = valves[4][value] == 0 ? Color.Red : Color.Green;
            v6.BackColor = valves[5][value] == 0 ? Color.Red : Color.Green;
            v7.BackColor = valves[6][value] == 0 ? Color.Red : Color.Green;
            v8.BackColor = valves[7][value] == 0 ? Color.Red : Color.Green;
            v9.BackColor = valves[8][value] == 0 ? Color.Red : Color.Green;
            v10.BackColor = valves[9][value] == 0 ? Color.Red : Color.Green;
            v11.BackColor = valves[10][value] == 0 ? Color.Red : Color.Green;
            v12.BackColor = valves[11][value] == 0 ? Color.Red : Color.Green;
            v13.BackColor = valves[12][value] == 0 ? Color.Red : Color.Green;
            v14.BackColor = valves[13][value] == 0 ? Color.Red : Color.Green;
            v15.BackColor = valves[14][value] == 0 ? Color.Red : Color.Green;
            v16.BackColor = valves[15][value] == 0 ? Color.Red : Color.Green;
            v17.BackColor = valves[16][value] == 0 ? Color.Red : Color.Green;
        }

        public void setLabels (int value, double[][] manos, double[][] temps, double[][] flowData)
        {
            pressureLabel1.Text = "Pressure : " + manos[0][value].ToString() + " Bar";
            pressureLabel2.Text = "Pressure : " + manos[1][value].ToString() + " Bar";
            pressureLabel3.Text = "Pressure : " + manos[2][value].ToString() + " Bar";

            temperatureLabel1.Text = "Temperature : " + temps[0][value].ToString() + " Celsius";
            temperatureLabel2.Text = "Temperature : " + temps[1][value].ToString() + " Celsius";
            temperatureLabel3.Text = "Temperature : " + temps[2][value].ToString() + " Celsius";

            litersLabel1.Text = flowData[0][value].ToString() + " Liters";
            litersLabel2.Text = flowData[1][value].ToString() + " Liters";
            litersLabel3.Text = flowData[2][value].ToString() + " Liters";
        }
    }
}
