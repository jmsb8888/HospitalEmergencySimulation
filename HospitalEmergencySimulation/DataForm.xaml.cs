using HospitalEmergencySimulation.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HospitalEmergencySimulation
{
    public partial class DataForm : Window
    {
        public DataForm()
        {
            InitializeComponent();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextNumeric(e.Text))
            {
                e.Handled = true;
            }
        }
        private bool IsTextNumeric(string text)
        {
            return double.TryParse(text, out _);
        }
        private void getParameters(object sender, RoutedEventArgs e)
        {

            double vMinimumAttentionTimeHighPriority = double.Parse(minimumAttentionTimeHighPriority.Text);
            double vMaximumAttentionTimeHighPriority = double.Parse(maximumAttentionTimeHighPriority.Text);
            double vMinimumAttentionTimeLowPriority = double.Parse(minimumAttentionTimeLowPriority.Text);
            double vMaximumAttentionTimeLowPriority = double.Parse(maximumAttentionTimeLowPriority.Text);
            double vLambdaArrivalHighPrority = double.Parse(lambdaArrivalHighPrority.Text);
            double vLambdaArrivalLowPrority = double.Parse(lambdaArrivalLowPrority.Text);
            int vNumberArrivalIntervals = int.Parse(numberArrivalIntervals.Text);

            ControllerSimulation controller = new ControllerSimulation(vMinimumAttentionTimeHighPriority, vMaximumAttentionTimeHighPriority, vMinimumAttentionTimeLowPriority, vMaximumAttentionTimeLowPriority,
            vLambdaArrivalHighPrority, vLambdaArrivalLowPrority, vNumberArrivalIntervals);
            MainWindow mainWindow = new MainWindow(controller);
            mainWindow.Show();
            this.Close();
        }

    }

}
