using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
    public class Distributions
    {
        /*Recibe un Ri y una lambda y con ello emplea la transformada inversa de poisson para
         * determinar cuántos pacientes llegaran en el tiempo establecido
         */
        public int PoissonInverseTransform(double Ri, double lam)
        {
            int Iterator = 0;
            // calculo de e^-Lamda
            double EulerData = Math.Exp(-lam);
            //    Console.WriteLine("dd " + EulerData + " sss " + Ri);
            double AuxEulerData = EulerData;
            while (true)
            {
                if (Ri < AuxEulerData)
                {
                    return Iterator;
                }
                else
                {
                    EulerData = EulerData * lam / (Iterator + 1);
                    AuxEulerData = AuxEulerData + EulerData;
                    Iterator = Iterator + 1;
                }
            }
        }
        /*Recibe un Ri y una lambda y con ello emplea la transformada inversa de la exponencial para
         * determinar un tiempo entre llegadas
         */
        public double ExponentialInverseTransform(double Ri, double lam)
        {
            return -Math.Log(1 - Ri) / lam;
        }
        /*Recibe un Ri, un límite inferior y un límite superior y con ello emplea la distribución uniforme  para
         * determinar un valor dado
         */
        public double UniformDistribution(double a, double b, double Ri)
        {
            return a + (b - a) * Ri;
        }
    }
}
