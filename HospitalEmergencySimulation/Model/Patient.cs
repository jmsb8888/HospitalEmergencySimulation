using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
    public class Patient
    {
        public int IdPatient { get; set; }
        public int Priority { get; set; }
        public Boolean IsAttended { get; set; }
        public double ServiceTime { get; set; }
        public double MissingServiceTime { get; set; }
        public double TimeOfArrival { get; set; }
        public int IdDoctor { get; set; }
        public double TimeOfExit { get; set; }
        public Boolean FinishedAttended { get; set; }

        public Patient(int idPatient, int priority)
        {
            IdPatient = idPatient;
            Priority = priority;
            IsAttended = false;
            IdDoctor = -1;
            FinishedAttended = false;
        }
        public Patient Clone()
        {
            return (Patient)this.MemberwiseClone();
        }
    }
}
