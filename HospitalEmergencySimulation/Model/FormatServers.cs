using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
   public class FormatServers
    {
        /*Datos de los servidores para la interfaz grafica
         */
        public int Coordinate { get; set; }
        public Boolean IsOcupped { get; set; }
        public int IdPatient { get; set; }
    }
}
