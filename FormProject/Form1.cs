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
using System.Threading;
using System.IO;
using MySqlConnector;

namespace h2AnalyticTool
{
    public partial class MainForm : Form
    {
        public System.Threading.Thread thread2;
        public System.Threading.ThreadStart threadParameters;
        public DateTime startTime;
        public DateTime endTime;
        public int speed;
        public int measureCount;
        public double[][] manos;
        public double[][] temps;
        public double[][] flowData;
        public DateTime[] time;
        public string[] stringTime;
        public TimeSpan totalTime;
        public int lastValue;
        public double currentSpeed = 1;
        public int[][] valves;
        //public pictureBoxForm picBox;
        public List<SessionInfo> sessionInfo;
        public SeriesList seriesList;
        public bool sessionLoaded;

        public MainForm()
        {
            InitializeComponent();
            this.sessionLoaded = false;
            this.seriesList = new SeriesList();
            this.seriesList.items = new Dictionary<int, Series>();
            this.sessionInfo = new List<SessionInfo>();
            addToConsole("Welcome to h2AnalyticTool");
            //picBox = new pictureBoxForm();
            //picBox.Show();
            this.lastValue = 0;
            threadParameters = new System.Threading.ThreadStart(delegate { startProgress(0, 0); });

            //chart 1
            Series series11 = new Series("Bar");
            series11.IsVisibleInLegend = false;
            series11.ChartType = SeriesChartType.Area;   
            
            Series series12 = new Series("Temperature");
            series12.IsVisibleInLegend = false;
            series12.ChartType = SeriesChartType.Line;
            series12.YAxisType = AxisType.Secondary;


            chart1.ChartAreas[0].AxisY.Title = "Bar";
           // chart1.ChartAreas[0].AxisY.Minimum = 0;
            //chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisY2.Title = "Celsius";
            chart1.ChartAreas[0].AxisY2.Minimum = -20;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY2.Maximum = 0;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
           

            chart1.ChartAreas[0].AxisX.ScaleView.Size = 50;

            seriesList.items.Add(11, series11);
            chart1.Series.Add(seriesList.items.First(x => x.Key == 11).Value);
            chart1.Series.Add(series12);
           
            //chart 2
            Series series21 = new Series("Bar");
            series21.IsVisibleInLegend = false;
            series21.ChartType = SeriesChartType.Area;
            Series series22 = new Series("Temperature");
            series22.IsVisibleInLegend = false;
            series22.ChartType = SeriesChartType.Line;
            series22.YAxisType = AxisType.Secondary;
            chart2.ChartAreas[0].AxisY.Title = "Bar";
            chart2.ChartAreas[0].AxisY.Minimum = 0;
            //chart2.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            chart2.ChartAreas[0].AxisY2.Title = "Celsius";
            chart2.ChartAreas[0].AxisY2.Minimum = -20;
            chart2.ChartAreas[0].AxisY2.Maximum = 0;
            chart2.ChartAreas[0].AxisY2.MajorGrid.LineWidth = 0;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart2.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;


            chart2.ChartAreas[0].AxisX.ScaleView.Size = 50;
            chart2.Series.Add(series21);
            chart2.Series.Add(series22);
            
            //chart 3
            Series series31 = new Series("Bar");
            series31.IsVisibleInLegend = false;
            series31.ChartType = SeriesChartType.Area;
            Series series32 = new Series("Temperature");
            series32.IsVisibleInLegend = false;
            series32.ChartType = SeriesChartType.Line;
            series32.YAxisType = AxisType.Secondary;
            chart3.ChartAreas[0].AxisY.Title = "Bar";
            chart3.ChartAreas[0].AxisY.Minimum = 0;
            //chart3.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            chart3.ChartAreas[0].AxisY2.Title = "Celsius";

            chart3.ChartAreas[0].AxisY2.Minimum = -20;
            chart3.ChartAreas[0].AxisY2.Maximum = 0;
            chart3.ChartAreas[0].AxisY2.MajorGrid.LineWidth = 0;
            chart3.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart3.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;

            chart3.ChartAreas[0].AxisX.ScaleView.Size = 50;
            chart3.Series.Add(series31);
            chart3.Series.Add(series32);

            //chart 4
            Series series41 = new Series("Liters");
            series41.IsVisibleInLegend = false;
            series41.ChartType = SeriesChartType.Line;
            chart4.ChartAreas[0].AxisY.Title = "Liters";
            chart4.ChartAreas[0].AxisX.ScaleView.Size = 50;
            chart4.Series.Add(series41);

            //chart 5
            Series series51 = new Series("Liters");
            series51.IsVisibleInLegend = false;
            series51.ChartType = SeriesChartType.Line;
            chart5.ChartAreas[0].AxisY.Title = "Liters";
            chart5.ChartAreas[0].AxisX.ScaleView.Size = 50;
            chart5.Series.Add(series51);

            //chart 6
            Series series61 = new Series("Liters");
            series61.IsVisibleInLegend = false;
            series61.ChartType = SeriesChartType.Line;
            chart6.ChartAreas[0].AxisY.Title = "Liters";
            chart6.ChartAreas[0].AxisX.ScaleView.Size = 50;
            chart6.Series.Add(series61);

            try
            {
                LoadSessionInfo();
                addToConsole("Database loaded.");
            } 
            catch (Exception ex)
            {
                addToConsole("Database error. Database not loaded");
                loadbutton.Enabled = false;
            }
            
        }

        public void addToConsole (string text)
        {
            richTextBox2.AppendText("\r\n" + text);
            richTextBox2.ScrollToCaret();
        }

        
        private void LoadSessionInfo()
        {
            string sql = "SELECT SUBSTRING(DATE_FORMAT(time, '%Y-%m-%d %T.%f'),1,23) sessionTime, name sessionName, id sessionId FROM session";
            using (MySqlConnection con = new MySqlConnection("SERVER=localhost;port=3306;database=h2database;uid=root;"))
            {
                MySqlCommand cmd = new MySqlCommand(sql, con);
                try
                {
                    con.Open();

                }
                catch (MySqlException ex)
                {
                    //When handling errors, you can your application's response based on the error number.
                    //The two most common error numbers when connecting are as follows:
                    //0: Cannot connect to server.
                    //1045: Invalid user name and/or password.
                    switch (ex.Number)
                    {
                        case 0:
                            addToConsole("Database not loaded. Cannot connect to server");
                            break;

                        case 1045:
                            addToConsole("Database not loaded. Invalid username/password, please try again");
                            break;
                        default:
                            addToConsole("Database error.");
                            break;
                    }

                }

                Dictionary<int, string> newDictionary = new Dictionary<int, string>();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SessionInfo sInfo = new SessionInfo();
                    sInfo.id = reader.GetInt32(reader.GetOrdinal("sessionId"));
                    sInfo.time = reader.GetString(reader.GetOrdinal("sessionTime"));
                    sInfo.sessionName = reader.GetString(reader.GetOrdinal("sessionName"));
                    newDictionary.Add(sInfo.id, sInfo.sessionName);
                    this.sessionInfo.Add(sInfo);
                }
                
                con.Close();
                sessionBox.DataSource = newDictionary.ToList();
                sessionBox.ValueMember = "Key";
                sessionBox.DisplayMember = "Value";
            }

        }


        private void LoadDataFromDB()
        {
            string sessiontime = sessionInfo.Find(x => x.id == (int)sessionBox.SelectedValue).time;
            string sqlCounter = "SELECT count(1) c from devicedata where id_session = " + sessionBox.SelectedValue;
            string sql = "SELECT SUBSTRING(DATE_FORMAT(time, '%T.%f'),1,12) stringtime , v.*, d.* FROM valvedata v, devicedata d where v.id = d.id and id_session = " + sessionBox.SelectedValue;
            int lineCount = 0;
            using (MySqlConnection con = new MySqlConnection("SERVER=localhost;port=3306;database=h2database;uid=root;"))
            {
                try
                {
                    con.Open();

                }
                catch (MySqlException ex)
                {
                    //When handling errors, you can your application's response based on the error number.
                    //The two most common error numbers when connecting are as follows:
                    //0: Cannot connect to server.
                    //1045: Invalid user name and/or password.
                    switch (ex.Number)
                    {
                        case 0:
                            addToConsole("Database not loaded. Cannot connect to server");
                            break;

                        case 1045:
                            addToConsole("Database not loaded. Invalid username/password, please try again");
                            break;
                        default:
                            addToConsole("Database error.");
                            break;
                    }

                }

                MySqlCommand cmd0 = new MySqlCommand(sqlCounter, con);
                MySqlDataReader reader0 = cmd0.ExecuteReader();
                

                while (reader0.Read())
                {
                    lineCount = reader0.GetInt32(reader0.GetOrdinal("c"));
                }
                con.Close();

            }

            using (MySqlConnection con = new MySqlConnection("SERVER=localhost;port=3306;database=h2database;uid=root;"))
            {
                
                try
                {
                    con.Open();

                }
                catch (MySqlException ex)
                {
                    //When handling errors, you can your application's response based on the error number.
                    //The two most common error numbers when connecting are as follows:
                    //0: Cannot connect to server.
                    //1045: Invalid user name and/or password.
                    switch (ex.Number)
                    {
                        case 0:
                            MessageBox.Show("Cannot connect to server.  Contact administrator");
                            break;

                        case 1045:
                            MessageBox.Show("Invalid username/password, please try again");
                            break;
                    }

                }

                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataReader reader = cmd.ExecuteReader();
                int i = 0;
                valves = new int[17][];
                manos = new double[3][];
                temps = new double[3][];
                flowData = new double[3][];
                time = new DateTime[lineCount];
                stringTime = new string[lineCount];

                for (int j = 0; j < 17; j++)
                {
                    valves[j] = new int[lineCount];
                }
                manos[0] = new double[lineCount];
                manos[1] = new double[lineCount];
                manos[2] = new double[lineCount];

                temps[0] = new double[lineCount];
                temps[1] = new double[lineCount];
                temps[2] = new double[lineCount];

                flowData[0] = new double[lineCount];
                flowData[1] = new double[lineCount];
                flowData[2] = new double[lineCount];


                while (reader.Read())
                {
                    valves[0][i] = reader.GetInt32(reader.GetOrdinal("valve1"));
                    valves[1][i] = reader.GetInt32(reader.GetOrdinal("valve2"));
                    valves[2][i] = reader.GetInt32(reader.GetOrdinal("valve3"));
                    valves[3][i] = reader.GetInt32(reader.GetOrdinal("valve4"));
                    valves[4][i] = reader.GetInt32(reader.GetOrdinal("valve5"));
                    valves[5][i] = reader.GetInt32(reader.GetOrdinal("valve6"));
                    valves[6][i] = reader.GetInt32(reader.GetOrdinal("valve7"));
                    valves[7][i] = reader.GetInt32(reader.GetOrdinal("valve8"));
                    valves[8][i] = reader.GetInt32(reader.GetOrdinal("valve9"));
                    valves[9][i] = reader.GetInt32(reader.GetOrdinal("valve10"));
                    valves[10][i] = reader.GetInt32(reader.GetOrdinal("valve11"));
                    valves[11][i] = reader.GetInt32(reader.GetOrdinal("valve12"));
                    valves[12][i] = reader.GetInt32(reader.GetOrdinal("valve13"));
                    valves[13][i] = reader.GetInt32(reader.GetOrdinal("valve14"));
                    valves[14][i] = reader.GetInt32(reader.GetOrdinal("valve15"));
                    valves[15][i] = reader.GetInt32(reader.GetOrdinal("valve16"));
                    valves[16][i] = reader.GetInt32(reader.GetOrdinal("valve17"));
                    time[i] = DateTime.Parse(reader.GetString(reader.GetOrdinal("stringtime")));
                    stringTime[i] = reader.GetString(reader.GetOrdinal("stringtime"));
                    manos[0][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("manometer1")),3);
                    manos[1][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("manometer2")), 3);
                    manos[2][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("manometer3")), 3);
                    temps[0][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("temperature1")), 3);
                    temps[1][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("temperature2")), 3);
                    temps[2][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("temperature3")), 3);
                    flowData[0][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("flowcounter1")), 3);
                    flowData[1][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("flowcounter2")), 3);
                    flowData[2][i] = Math.Round(reader.GetDouble(reader.GetOrdinal("flowcounter3")), 3);
                    i++;
                }


                startTime = time[0];
                endTime = time[time.Length - 1];
                totalTime = endTime - startTime;
                measureCount = lineCount;
            }

            this.sessionLoaded = true;
        }

        
        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {


        }

        private void playbutton_click(object sender, EventArgs e)
        {
            if (!sessionLoaded) { addToConsole("Session no loaded. Load session before starting process!"); return; }

            comboBox1.SelectedIndex = 4;
            loadbutton.Enabled = false;
            threadParameters = new System.Threading.ThreadStart(delegate { startProgress(0, this.lastValue); });
            thread2 = new System.Threading.Thread(threadParameters);
            thread2.Start();
            addToConsole("The process has started.");
        }


        public void startProgress(int incr, int value)
        {            
            if (textBox1.InvokeRequired)
            {
                for (int i = value; i < measureCount; i++)
                {
                    TimeSpan fromStart = time[i] - startTime;
                    int percentage = Convert.ToInt32(Math.Floor(fromStart.TotalMilliseconds / totalTime.TotalMilliseconds * 100));
                    int newIncr = 0;
                    if (percentage > progressBar1.Value) newIncr = 1;
                    // Call this same method but append THREAD2 to the text
                    Action safeWrite = delegate { startProgress(newIncr, i); };
                    textBox1.Invoke(safeWrite);
                    if (i + 1 < time.Length)
                    {
                        TimeSpan result = time[i + 1] - time[i];
                        Thread.Sleep(Convert.ToInt32(Math.Round(result.Milliseconds * currentSpeed)));
                    }

                }
            }
            else
            {
                setProgressIncrement(incr, value);
            }
        }

        private void pausebutton_click(object sender, EventArgs e)
        {
            if (!sessionLoaded) 
            { 
                addToConsole("Session no loaded. Load session before starting process!"); 
                return; 
            }
            
            addToConsole("The process is suspended.");
            thread2.Abort();
        }

        private void restorebutton_click(object sender, EventArgs e)
        {
            if (!sessionLoaded) 
            { 
                addToConsole("Session no loaded. Load session before starting process!"); 
                return; 
            }
            
            thread2.Abort();
            
            progressBar1.Value = 0;
            
            try
            {
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
            }
            catch (Exception ex) { }
            try
            {
                foreach (var series in chart2.Series)
                {
                    series.Points.Clear();
                }
            }
            catch (Exception ex) { }
            try
            {
                foreach (var series in chart3.Series)
            {
                series.Points.Clear();
            }
            }
            catch (Exception ex) { }
            try
            {
                foreach (var series in chart4.Series)
            {
                series.Points.Clear();
            }
            }
            catch (Exception ex) { }
            try
            {
                foreach (var series in chart5.Series)
            {
                series.Points.Clear();
            }
            }
            catch (Exception ex) { }
            try
            {
                foreach (var series in chart6.Series)
            {
                series.Points.Clear();
            }
            }
            catch (Exception ex) { }
            
            this.lastValue = 0;

            chart1.ChartAreas[0].AxisX.ScaleView.Position = 0;
            chart2.ChartAreas[0].AxisX.ScaleView.Position = 0;
            chart3.ChartAreas[0].AxisX.ScaleView.Position = 0;
            chart4.ChartAreas[0].AxisX.ScaleView.Position = 0;
            chart5.ChartAreas[0].AxisX.ScaleView.Position = 0;
            chart6.ChartAreas[0].AxisX.ScaleView.Position = 0;

            v1.BackColor = Color.Green;
            v2.BackColor = Color.Green;
            v3.BackColor = Color.Green;
            v4.BackColor = Color.Green;
            v5.BackColor = Color.Green;
            v6.BackColor = Color.Green;
            v7.BackColor = Color.Green;
            v8.BackColor = Color.Green;
            v9.BackColor = Color.Green;
            v10.BackColor = Color.Green;
            v11.BackColor = Color.Green;
            v12.BackColor = Color.Green;
            v13.BackColor = Color.Green;
            v14.BackColor = Color.Green;
            v15.BackColor = Color.Green;
            v16.BackColor = Color.Green;
            v17.BackColor = Color.Green;


            addToConsole("The process is restarted from the beginning.");

            pressureLabel1.Text = "Pressure: - Bar";
            pressureLabel2.Text = "Pressure: - Bar";
            pressureLabel3.Text = "Pressure: - Bar";
            pressureLabel12.Text = "- Bar";
            pressureLabel22.Text = "- Bar";
            pressureLabel32.Text = "- Bar";

            temperatureLabel1.Text = "Temperature: - Celsius";
            temperatureLabel2.Text = "Temperature: - Celsius";
            temperatureLabel3.Text = "Temperature: - Celsius";
            temperatureLabel12.Text = "- Celsius";
            temperatureLabel22.Text = "- Celsius";
            temperatureLabel32.Text = "- Celsius";

            litersLabel1.Text = "- Liters";
            litersLabel2.Text = "- Liters";
            litersLabel3.Text = "- Liters";
            litersLabel12.Text = "- Liters";
            litersLabel22.Text = "- Liters";
            litersLabel32.Text = "- Liters";

            progressLabel.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void setProgressIncrement(int incr, int value)
        {
            double aPoint = progressBar1.Width / 100;
            int pointsToRight = Convert.ToInt32(Math.Round(aPoint * (progressBar1.Value + 1)));
            progressBar1.Increment(incr);
            progressLabel.Location = new Point(progressBar1.Left + pointsToRight, progressLabel.Location.Y);
            progressLabel.Text = stringTime[value];

            addManoPoint(chart1, value, 0); 
            addManoPoint(chart2, value, 1); 
            addManoPoint(chart3, value, 2);

            pressureLabel1.Text = "Pressure: " + manos[0][value].ToString() + " Bar";
            pressureLabel2.Text = "Pressure: " + manos[1][value].ToString() + " Bar";
            pressureLabel3.Text = "Pressure: " + manos[2][value].ToString() + " Bar";


            pressureLabel12.Text = manos[0][value].ToString() + " Bar";
            pressureLabel22.Text = manos[1][value].ToString() + " Bar";
            pressureLabel32.Text = manos[2][value].ToString() + " Bar";

            addTemperaturePoint(chart1, value, 0); 
            addTemperaturePoint(chart2, value, 1); 
            addTemperaturePoint(chart3, value, 2);

            temperatureLabel1.Text = "Temperature: " + temps[0][value].ToString() + " Celsius";
            temperatureLabel2.Text = "Temperature: " + temps[1][value].ToString() + " Celsius";
            temperatureLabel3.Text = "Temperature: " + temps[2][value].ToString() + " Celsius";

            temperatureLabel12.Text = temps[0][value].ToString() + " Celsius";
            temperatureLabel22.Text = temps[1][value].ToString() + " Celsius";
            temperatureLabel32.Text = temps[2][value].ToString() + " Celsius";

            //picBox.setLabels(value, manos, temps, flowData);

            addFlowDataPoint(chart4, value, 0);
            addFlowDataPoint(chart5, value, 1);
            addFlowDataPoint(chart6, value, 2);

            litersLabel1.Text = flowData[0][value].ToString() + " Liters";
            litersLabel2.Text = flowData[1][value].ToString() + " Liters";
            litersLabel3.Text = flowData[2][value].ToString() + " Liters";

            litersLabel12.Text = flowData[0][value].ToString() + " Liters";
            litersLabel22.Text = flowData[1][value].ToString() + " Liters";
            litersLabel32.Text = flowData[2][value].ToString() + " Liters";

            setValves(value);
            
            fixChartArea();           
            this.lastValue = value;
            if (value == measureCount - 1)
            {
                loadbutton.Enabled = true;
                addToConsole("Process completed successfully!");
            }
                
        }


        public void setValves(int value)
        {
            //picBox.setValves(value, valves);
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

        public void addManoPoint(Chart c, int value, int mano)
        {
            DataPoint newDataPoint = new DataPoint(value, manos[mano][value]);
            newDataPoint.AxisLabel = stringTime[value];
            c.Series[0].Points.Add(newDataPoint);
        }

        public void addTemperaturePoint(Chart c, int value, int temp)
        {
            DataPoint newDataPoint = new DataPoint(value, temps[temp][value]);
            newDataPoint.AxisLabel = stringTime[value];
            c.Series[1].Points.Add(newDataPoint);            
        }

        public void addFlowDataPoint(Chart c, int value, int flow)
        {
            DataPoint newDataPoint = new DataPoint(value, flowData[flow][value]);
            newDataPoint.AxisLabel = stringTime[value];
            c.Series[0].Points.Add(newDataPoint);
            
        }
        public void fixChartArea ()
        {
            chart1.ChartAreas[0].AxisX.ScaleView.Position = chart1.Series[0].Points.Count > 50 ? chart1.Series[0].Points.Count - 50 : chart1.ChartAreas[0].AxisX.ScaleView.Position;
            chart2.ChartAreas[0].AxisX.ScaleView.Position = chart2.Series[0].Points.Count > 50 ? chart2.Series[0].Points.Count - 50 : chart2.ChartAreas[0].AxisX.ScaleView.Position;
            chart3.ChartAreas[0].AxisX.ScaleView.Position = chart3.Series[0].Points.Count > 50 ? chart3.Series[0].Points.Count - 50 : chart3.ChartAreas[0].AxisX.ScaleView.Position;
            chart4.ChartAreas[0].AxisX.ScaleView.Position = chart4.Series[0].Points.Count > 50 ? chart4.Series[0].Points.Count - 50 : chart4.ChartAreas[0].AxisX.ScaleView.Position;
            chart5.ChartAreas[0].AxisX.ScaleView.Position = chart5.Series[0].Points.Count > 50 ? chart5.Series[0].Points.Count - 50 : chart5.ChartAreas[0].AxisX.ScaleView.Position;
            chart6.ChartAreas[0].AxisX.ScaleView.Position = chart6.Series[0].Points.Count > 50 ? chart6.Series[0].Points.Count - 50 : chart6.ChartAreas[0].AxisX.ScaleView.Position;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
               

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSpeed = double.Parse(comboBox1.SelectedItem.ToString().Replace('.',','));
        }

        private void showPopupOnDblclick(object sender, EventArgs e)
        {
            popupForm frm = new popupForm((Chart)sender);
            frm.Show();
        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void chart3_DoubleClick(object sender, EventArgs e)
        {
            popupForm frm = new popupForm((Chart)sender, 0);
            frm.Show();
        }

        private void loadbutton_click(object sender, EventArgs e)
        {
            try
            {
                LoadDataFromDB();
                SessionInfo currentSession = sessionInfo.Find(x => x.id == (int)sessionBox.SelectedValue);
                addToConsole("You selected '" + currentSession.sessionName + "' and creation date is '" + currentSession.time);
            }
            catch (Exception ex)
            {
                addToConsole("Database error. Session not loaded.");
            }
        }
    }

    public class SessionInfo
    {
        public string time { get; set; }
        public int id { get; set; }

        public string sessionName { get; set; }
    }

    public class SeriesList
    {
        public Dictionary<int, Series> items { get; set; }
    }
}
