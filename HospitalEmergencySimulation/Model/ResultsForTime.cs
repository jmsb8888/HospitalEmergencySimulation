using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
    public class ResultsForTime
    {
        /*Formato que guardara los registros de la simulación en cada instante de tiempo a analizar
         */
        public int Time { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Patient> PatientsAttended { get; set; }
        public List<Patient> PatientsInSystem { get; set; }
        public ResultsForTime()
        {
            Doctors = new List<Doctor>();
            PatientsAttended = new List<Patient>();
            PatientsInSystem = new List<Patient>();

        }

    }
}

