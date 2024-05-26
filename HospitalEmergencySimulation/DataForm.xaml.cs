using HospitalEmergencySimulation.Controller;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            string newText = ((TextBox)sender).Text + e.Text;
            e.Handled = !IsTextNumeric(newText);
        }

        private bool IsTextNumeric(string text)
        {
            bool isNumeric = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out _);
            return isNumeric || text == ".";
        }
        private void TextBoxx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IssTextNumeric(e.Text))
            {
                e.Handled = true;
            }
        }
        private bool IssTextNumeric(string text)
        {
            bool isNumeric = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out _);
            return isNumeric;
        }
        private void getParameters(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(minimumAttentionTimeHighPriority.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double vMinimumAttentionTimeHighPriority) &&
                double.TryParse(maximumAttentionTimeHighPriority.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double vMaximumAttentionTimeHighPriority) &&
                double.TryParse(minimumAttentionTimeLowPriority.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double vMinimumAttentionTimeLowPriority) &&
                double.TryParse(maximumAttentionTimeLowPriority.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double vMaximumAttentionTimeLowPriority) &&
                double.TryParse(lambdaArrivalHighPrority.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double vLambdaArrivalHighPrority) &&
                double.TryParse(lambdaArrivalLowPrority.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double vLambdaArrivalLowPrority) &&
                int.TryParse(numberArrivalIntervals.Text, out int vNumberArrivalIntervals))
            {
                ControllerSimulation controller = new ControllerSimulation(vMinimumAttentionTimeHighPriority, vMaximumAttentionTimeHighPriority, vMinimumAttentionTimeLowPriority, vMaximumAttentionTimeLowPriority,
                    vLambdaArrivalHighPrority, vLambdaArrivalLowPrority, vNumberArrivalIntervals);
                MainWindow mainWindow = new MainWindow(controller);
                mainWindow.Show();
                this.Close();
            }
        }

        }

}
