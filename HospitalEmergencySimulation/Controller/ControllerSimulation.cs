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
            this.minimumAttentionTimeHighPriority = 3;
            this.maximumAttentionTimeHighPriority = 5;
            this.minimumAttentionTimeLowPriority = 1;
            this.maximumAttentionTimeLowPriority = 2;
            this.lambdaArrivalHighPrority = 1;
            this.lambdaArrivalLowPrority = 0.8;
            this.numberArrivalIntervals = 5;
            this.queueSimulationManager = new QueueSimulationManager(this.minimumAttentionTimeHighPriority,this.maximumAttentionTimeHighPriority, this.minimumAttentionTimeLowPriority,this.maximumAttentionTimeLowPriority,
                this.lambdaArrivalHighPrority, this.lambdaArrivalLowPrority, this.numberArrivalIntervals);
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
