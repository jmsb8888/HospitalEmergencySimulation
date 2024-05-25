using HospitalEmergencySimulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;


namespace HospitalEmergencySimulation.Controller
{
    public class ControllerSimulation
    {
        private QueueSimulationManager queueSimulationManager;
        private int minimumAttentionTimeHighPriority;
        private int maximumAttentionTimeHighPriority;
        private int minimumAttentionTimeLowPriority;
        private int maximumAttentionTimeLowPriority;
        private int lambdaArrivalHighPrority;
        private int lambdaArrivalLowPrority;
        private int numberArrivalIntervals;
        ObservableCollection<ResultsForTime> results = new ObservableCollection<ResultsForTime>();
        public ControllerSimulation(int minimumAttentionTimeHighPriority, int maximumAttentionTimeHighPriority, int minimumAttentionTimeLowPriority, int maximumAttentionTimeLowPriority,
                                      int lambdaArrivalHighPrority, int lambdaArrivalLowPrority, int numberArrivalIntervals)
        {
            this.minimumAttentionTimeHighPriority = minimumAttentionTimeHighPriority;
            this.maximumAttentionTimeHighPriority = maximumAttentionTimeHighPriority;
            this.minimumAttentionTimeLowPriority = minimumAttentionTimeLowPriority;
            this.maximumAttentionTimeLowPriority = maximumAttentionTimeLowPriority;
            this.lambdaArrivalHighPrority = lambdaArrivalHighPrority;
            this.lambdaArrivalLowPrority = lambdaArrivalLowPrority;
            this.numberArrivalIntervals = numberArrivalIntervals;
            this.queueSimulationManager = new QueueSimulationManager(minimumAttentionTimeHighPriority,maximumAttentionTimeHighPriority, minimumAttentionTimeLowPriority,maximumAttentionTimeLowPriority,
                lambdaArrivalHighPrority, lambdaArrivalLowPrority, numberArrivalIntervals);
        }

       public ObservableCollection<ResultsForTime> GetResults()
        {
            int count = 0;
            while(count < 30)
            {
                queueSimulationManager.init();
                count++;
            }
            count = 0;
            while(count == 0)
            {
                count = queueSimulationManager.FinishAttention();
            }
            results = queueSimulationManager.GetResultForTimes();
            MessageBox.Show("quedo en " + queueSimulationManager.CurrentSimulationTime);
            return results;
        }

        public void print()
        {
            queueSimulationManager.PrintLists();
        }
    }
}
