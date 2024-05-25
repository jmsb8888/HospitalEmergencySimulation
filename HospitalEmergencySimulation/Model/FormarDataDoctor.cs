using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
    class FormarDataDoctor
    {
        public int TimeSimulation {  get; set; }
        public int IdDoctor { get; set; }
        public Boolean IsOccupied { get; set; }
        public double Time { get; set; }
        public int IdPatient { get; set; }

    }
}
