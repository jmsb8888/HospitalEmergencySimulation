using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
    public class FileHandler
    {
        string filePath;
        List<double> data = new List<double>();

        public FileHandler()
        {
            this.filePath = "C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\numeros_ri_proyecto.csv";
        }

        public List<double> ReadCsvFile()
        {
            List<double> data = new List<double>();
            using (TextFieldParser parser = new TextFieldParser(this.filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                while (!parser.EndOfData)
                {
                    string[] fila = parser.ReadFields();
                    foreach (string s in fila)
                    {
                        double aux = double.Parse(s, CultureInfo.InvariantCulture);
                        data.Add(aux);
                    }
                }
            }

            return data;
        }
    }
}
