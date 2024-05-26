using HospitalEmergencySimulation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ViewTables : Window
    {
        ObservableCollection<ResultsForTime> data;
        public ViewTables(ObservableCollection<ResultsForTime> data)
        {
            this.data = data;
            InitializeComponent();
            InitSimulation();
        }

        public void InitSimulation()
        {
            
            List<FormarDataDoctor> DataDoctor = new List<FormarDataDoctor>();
            List<FormatPatient> formatPatients = new List<FormatPatient>();
           ;
            foreach (ResultsForTime result in data)
            {

                foreach (Doctor result2 in result.Doctors)
                {
                        FormarDataDoctor aux = new FormarDataDoctor
                        {
                            TimeSimulation = result.Time,
                            IdDoctor = result2.IdDoctor,
                            IdPatient = result2.IdPatient,
                            IsOccupied = result2.IsOccupied,
                            Time = result2.Time
                        };
                        DataDoctor.Add(aux);
                    

                }
                foreach (Patient result2 in result.PatientsInSystem)
                {

                        FormatPatient aux = new FormatPatient
                        {
                            TimeSimulation = result.Time,
                            IdDoctor = result2.IdDoctor,
                            IdPatient = result2.IdPatient,
                            IsAttended = result2.IsAttended,
                            ServiceTime = result2.ServiceTime,
                            MissingServiceTime = result2.MissingServiceTime,
                            TimeOfArrival = result2.TimeOfArrival,
                            TimeOfExit = result2.TimeOfExit,
                            FinishedAttended = result2.FinishedAttended,
                            Priority = result2.Priority,
                            TimeWait = result2.TimeWait,
                        };
                        formatPatients.Add(aux);
                }

            }
           
            ResultsPatient.ItemsSource = formatPatients;
            ResultsForTimeDataGrid.ItemsSource = DataDoctor;
        }
    }
}
