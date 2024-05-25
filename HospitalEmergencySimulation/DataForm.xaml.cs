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
    /// <summary>
    /// Lógica de interacción para DataForm.xaml
    /// </summary>
    public partial class DataForm : Window
    {
        public DataForm()
        {
            InitializeComponent();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Comprobar si el texto ingresado es numérico
            if (!IsTextNumeric(e.Text))
            {
                // Si no es numérico, cancelar el evento
                e.Handled = true;
            }
        }
        private bool IsTextNumeric(string text)
        {
            // Intentar convertir el texto a un número
            return double.TryParse(text, out _);
        }
        private void getParameters(object sender, RoutedEventArgs e)
        {
            
            int vMinimumAttentionTimeHighPriority = int.Parse(minimumAttentionTimeHighPriority.Text);
            int vMaximumAttentionTimeHighPriority = int.Parse(maximumAttentionTimeHighPriority.Text);
            int vMinimumAttentionTimeLowPriority = int.Parse(minimumAttentionTimeLowPriority.Text);
            int vMaximumAttentionTimeLowPriority = int.Parse(maximumAttentionTimeLowPriority.Text);
            int vLambdaArrivalHighPrority = int.Parse(lambdaArrivalHighPrority.Text);
            int vLambdaArrivalLowPrority = int.Parse(lambdaArrivalLowPrority.Text);
            int vNumberArrivalIntervals = int.Parse(numberArrivalIntervals.Text);

            ControllerSimulation controller = new ControllerSimulation(vMinimumAttentionTimeHighPriority, vMaximumAttentionTimeHighPriority, vMinimumAttentionTimeLowPriority, vMaximumAttentionTimeLowPriority,
                vLambdaArrivalHighPrority, vLambdaArrivalLowPrority, vNumberArrivalIntervals);
            MainWindow mainWindow = new MainWindow(controller);
            mainWindow.Show();
            this.Close();
        }
        
    }

}
