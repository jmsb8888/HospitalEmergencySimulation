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
        private double minimumAttentionTimeHighPriority;
        private double maximumAttentionTimeHighPriority;
        private double minimumAttentionTimeLowPriority;
        private double maximumAttentionTimeLowPriority;
        private double lambdaArrivalHighPrority;
        private double lambdaArrivalLowPrority;
        private int numberArrivalIntervals;
        ObservableCollection<ResultsForTime> results = new ObservableCollection<ResultsForTime>();

        public ControllerSimulation(double minimumAttentionTimeHighPriority, double maximumAttentionTimeHighPriority, double minimumAttentionTimeLowPriority, double maximumAttentionTimeLowPriority,
                                              double lambdaArrivalHighPrority, double lambdaArrivalLowPrority, int numberArrivalIntervals)
        {
            this.minimumAttentionTimeHighPriority = minimumAttentionTimeHighPriority;
            this.maximumAttentionTimeHighPriority = maximumAttentionTimeHighPriority;
            this.minimumAttentionTimeLowPriority = minimumAttentionTimeLowPriority;
            this.maximumAttentionTimeLowPriority = maximumAttentionTimeLowPriority;
            this.lambdaArrivalHighPrority = lambdaArrivalHighPrority;
            this.lambdaArrivalLowPrority = lambdaArrivalLowPrority;
            this.numberArrivalIntervals = numberArrivalIntervals;
            this.queueSimulationManager = new QueueSimulationManager(minimumAttentionTimeHighPriority, maximumAttentionTimeHighPriority, minimumAttentionTimeLowPriority, maximumAttentionTimeLowPriority,
                lambdaArrivalHighPrority, lambdaArrivalLowPrority, numberArrivalIntervals);
        }

        /*Se obtienen resultados tanto del tiempo de simulación establecido como del tiempo de atención final
        */
        public ObservableCollection<ResultsForTime> GetResults()
        {
            int count = 0;
            while(count < 5)
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
            return results;
        }
    }
}
