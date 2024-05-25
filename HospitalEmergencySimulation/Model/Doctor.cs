using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
    public class Doctor
    {
        public int IdDoctor { get; set; }
        public Boolean IsOccupied { get; set; }
        public double Time { get; set; }
        public int IdPatient { get; set; }

        public Doctor(int idDoctor)
        {
            IdDoctor = idDoctor;
            IsOccupied = false; ;
            IdPatient = -1;
            IsOccupied = false;
        }
        public Doctor Clone()
        {
            return (Doctor)this.MemberwiseClone();
        }
    }
}
