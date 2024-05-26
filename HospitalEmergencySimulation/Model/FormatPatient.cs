using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
    public class FormatPatient
    {
        /*Formato para mostrar los datos de los pacientes durante la simulación 
    */
        public int TimeSimulation {  get; set; }
        public int IdPatient { get; set; }
        public int Priority { get; set; }
        public Boolean IsAttended { get; set; }
        public double ServiceTime { get; set; }
        public double MissingServiceTime { get; set; }
        public double TimeOfArrival { get; set; }
        public double TimeWait { get; set; }
        public int IdDoctor { get; set; }
        public double TimeOfExit { get; set; }
        public Boolean FinishedAttended { get; set; }
    }
}
