using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalEmergencySimulation.Model
{
    internal class QueueSimulationManager
    {
        public int NumberOfPatients {  get; set; }
        public int TotalArrivalTime {  get; set; }
        double TimeTraveled {  get; set; }
        int CurrentSimulationTime {  get; set; }
        Boolean isOccuped = false;
        Boolean isCheckPatient = false;
        Distributions distributions = new Distributions();
        List<Patient> HighPriorityQueue = new List<Patient>();
        List<Patient> LowPriorityQueue = new List<Patient>();
        List<Patient> PatientsTreatedHighPriority = new List<Patient>();
        List<Patient> PatientsTreatedLowPriority = new List<Patient>();
        List<Doctor> doctors = new List<Doctor>();
        
        public QueueSimulationManager() 
        {
            NumberOfPatients =0;
            TotalArrivalTime = 20;
            TimeTraveled = 0;
            CurrentSimulationTime = 0;
            createDoctors(3);
            init();
        }
        public void init()
        {

            while (CurrentSimulationTime <= TotalArrivalTime)
            {
                CurrentSimulationTime++;
                GenerateLowPriorityQueue();
                GenerateHighPriorityQueue(CurrentSimulationTime);
                ManageCustomerService();
                Console.WriteLine(CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime);
                foreach (Doctor doctor in doctors)
                {
                    int d = doctor.IdPatient;
                    Patient f = LowPriorityQueue.Find(c => c.IdPatient == d);
                    if (f == null)
                    {
                        f = HighPriorityQueue.Find(c => c.IdPatient == d);
                    }
                    if (f == null)
                    {
                        f = PatientsTreatedHighPriority.Find(c => c.IdPatient == d);
                    }
                    if (f == null)
                    {
                        f = PatientsTreatedLowPriority.Find(c => c.IdPatient == d);
                    }
                    if (doctor.IdPatient == -1)
                    {
                        Console.WriteLine(doctor.IdDoctor + " " + "paciente  " + doctor.IdPatient + " no hay paciente " + " *************");
                    }
                    else
                    {
                        Console.WriteLine(doctor.IdDoctor + " " + "paciente  " + doctor.IdPatient + " tiempo paciente " + f.MissingServiceTime + " *************");

                    }
                }
            }
            while (HighPriorityQueue.Count > 0 || LowPriorityQueue.Count > 0)
            {
                ManageCustomerService();
                Console.WriteLine(CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime);
                foreach (Doctor doctor in doctors)
                {
                    int d = doctor.IdPatient;
                    Patient f = LowPriorityQueue.Find(c => c.IdPatient == d);
                    if (f == null)
                    {
                        f = HighPriorityQueue.Find(c => c.IdPatient == d);
                    }
                    if (f == null)
                    {
                        f = PatientsTreatedHighPriority.Find(c => c.IdPatient == d);
                    }
                    if (f == null)
                    {
                        f = PatientsTreatedLowPriority.Find(c => c.IdPatient == d);
                    }
                    if (doctor.IdPatient == -1)
                    {
                        Console.WriteLine(doctor.IdDoctor + " " + "paciente  " + doctor.IdPatient + " no hay paciente " + " *************");
                    }
                    else
                    {
                        Console.WriteLine(doctor.IdDoctor + " " + "paciente  " + doctor.IdPatient + " tiempo paciente " + f.MissingServiceTime + " *************");

                    }
                }
            }
            /*
            int num = 1;
            while (num == 1)
            {
                num = 
            }
            for (int i = 0;i < 20;i++)
            {
                
            }
            for (int i = 0; i < 20; i++)
            {
                asignartiempoServicio();
                
            }
            Boolean f = true;
            
            {
                atencionBaja();
              //  f= checkatencionBAja();
            }*/


        }
        public void PrintLists()
        {
            Console.WriteLine("Pacientes Alta Prioridad:");
            foreach (var patient in HighPriorityQueue)
            {
                Console.WriteLine($"IdPatient: {patient.IdPatient}, Priority: {patient.Priority}, Atentido: {patient.IsAttended}, TimefaltanteServicio: {patient.MissingServiceTime}, TimeLLegada: {patient.TimeOfArrival}, tIEMPOSERVICIO {patient.ServiceTime}");
            }

            Console.WriteLine("\nPacientes Baja Prioridad:");
            foreach (var patient in LowPriorityQueue)
            {
                Console.WriteLine($"IdPatient: {patient.IdPatient}, Priority: {patient.Priority}, Atentido: {patient.IsAttended}, TimefaltanteServicio: {patient.MissingServiceTime}, TimeLLegada: {patient.TimeOfArrival}, tIEMPOSERVICIO {patient.ServiceTime}");
            }

            Console.WriteLine("\nDoctores:");
            foreach (var doctor in doctors)
            {
                Console.WriteLine($"IdDoctor: {doctor.IdDoctor}, Ocupado: {doctor.IsOccupied}, Time: {doctor.Time}");
            }

            Console.WriteLine("\natendidos alta:");
            foreach (var patient in PatientsTreatedHighPriority)
            {
                Console.WriteLine($"IdPatient: {patient.IdPatient}, Priority: {patient.Priority}, Atentido: {patient.IsAttended}, TimefaltanteServicio: {patient.MissingServiceTime}, TimeLLegada: {patient.TimeOfArrival}, tIEMPOSERVICIO {patient.ServiceTime}");
            }

            Console.WriteLine("\n\n atendidos baja:");
            foreach (var patient in PatientsTreatedLowPriority)
            {
                Console.WriteLine($"IdPatient: {patient.IdPatient}, Priority: {patient.Priority}, Atentido: {patient.IsAttended}, TimefaltanteServicio: {patient.MissingServiceTime}, TimeLLegada: {patient.TimeOfArrival}, tIEMPOSERVICIO {patient.ServiceTime}");
            }

            Console.WriteLine("quedan " + LowPriorityQueue.Count);
        }

        private void createDoctors(int NumberOfDoctors)
        {
            for (int i = 0; i < NumberOfDoctors; i++)
            {
                Doctor doctor = new Doctor(i + 1);
                doctors.Add(doctor);
            }
        }
        private void CreatePatient(double timeArrival, int priority, List<Patient> queue, Action<Patient> AssignSeriviceTime)
        {
            NumberOfPatients++;
            Patient patient = new Patient(NumberOfPatients, priority);
            patient.TimeOfArrival = timeArrival;
            AssignSeriviceTime(patient);
            queue.Add(patient);
        }

        public void GenerateHighPriorityQueue(int count)
        {
            double Lambda = 0.05;
            double Ri = new Random().NextDouble();
            int PoissonValue = distributions.PoissonInverseTransform(Ri, Lambda);
            if (PoissonValue != 0)
            {
                for (int i = 0; i < PoissonValue; i++)
                {
                    CreatePatient(count, 1, HighPriorityQueue, (patient) => AssignSeriviceTime(patient, 8, 15));
                }
            }
            Console.WriteLine("ddddddddddddddddddddddddd " + HighPriorityQueue.Count);
        }
        public int GenerateLowPriorityQueue()
        {
            int count = 0;
            //Ri para exponencial
            double Ri = new Random().NextDouble();
            //Lamda para expoenencial clientes por unidad de tiempo
            int ArrivalRate = 2;
            double ExponentialValue = distributions.ExponentialInverseTransform(Ri, ArrivalRate);

            if (TimeTraveled <= CurrentSimulationTime)
            {
                CreatePatient(TimeTraveled, 0, LowPriorityQueue, (patient) => AssignSeriviceTime(patient, 3, 7));
                TimeTraveled += ExponentialValue;
                Console.WriteLine("cccccccccccccccccccccccccc " + LowPriorityQueue.Count);
                return 1;
            };
            return 0;
        }

        public void AssignSeriviceTime(Patient patient, int lowerLimit, int highLimit)
        {
            double Ri = new Random().NextDouble();
            double UniformValue = distributions.UniformDistribution(lowerLimit, highLimit, Ri);
            Console.WriteLine("Asignar tiempo de servicio: " + UniformValue);
            patient.MissingServiceTime = UniformValue;
            patient.ServiceTime = UniformValue;
        }
        public void ManageCustomerService()
        { 
            for (int j = 0; j < doctors.Count; j++)
            {
                Doctor doctor = doctors[j];
                isOccuped = false;
                bool wasOccupied = false;
                if (!wasOccupied)
                {
                   CheckQueue(this.HighPriorityQueue, doctor, "High");
                    wasOccupied = isOccuped;
                }
                if (!wasOccupied)
                {
                   bool nextDoctor= CheckQueue(LowPriorityQueue, doctor, "Lower");
                    Console.WriteLine("mmmmmmmmmmmmmmmmmmmmmm "+ LowPriorityQueue.Count);
                   /* Console.WriteLine("\nPacientes Baja Prioridad:");
                    foreach (var patient in LowPriorityQueue)
                    {
                        Console.WriteLine($"IdPatient: {patient.IdPatient}, Priority: {patient.Priority}, Atentido: {patient.IsAttended}, TimefaltanteServicio: {patient.MissingServiceTime}, TimeLLegada: {patient.TimeOfArrival}, tIEMPOSERVICIO {patient.ServiceTime}");
                    }*/
                    if (!nextDoctor)
                    {
                        j--;
                        
                    }
                }
            }
        }
            
        
       
        public bool CheckQueue(List<Patient> queue, Doctor doctor, String priority)
        {
            bool IsCheck = false;
            if (queue.Count > 0)
            {
                for (int i = 0; i < queue.Count; i++)
                {
                    isCheckPatient = false;
                    Patient patient = queue[i];
                    if (!doctor.IsOccupied && !patient.IsAttended)
                    {
                        patient.IsAttended = true;
                        // wasOccupied = true;
                        doctor.IdPatient = patient.IdPatient;
                        patient.IdDoctor = doctor.IdDoctor;
                        doctor.IsOccupied = true;
                        isOccuped = true;
                        break;
                    }
                    else if (patient.IdDoctor == doctor.IdDoctor && patient.IsAttended && patient.MissingServiceTime > 0)
                    {
                        patient.MissingServiceTime -= 1;
                        isCheckPatient = true;
                        //IsCheck = true;

                    }
                    if (patient.IdDoctor == doctor.IdDoctor && patient.IsAttended && patient.MissingServiceTime <= 0)
                    {
                        doctor.IsOccupied = false;
                        //(priority == "High" ? PatientsTreatedHighPriority : PatientsTreatedLowPriority).Add(patient);
                        if (priority == "High")
                        {
                            Console.WriteLine("************************************************************");
                            PatientsTreatedHighPriority.Add(patient);
                            HighPriorityQueue.Remove(patient);
                        }
                        else
                        {
                            Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
                            PatientsTreatedLowPriority.Add(patient);
                            LowPriorityQueue.Remove(patient);
                        }
                        
                        IsCheck = true;
                        break;
                    }
                    if (isCheckPatient)
                    {
                        break;
                    }
                }
            }
            if (IsCheck)
            {
                return false;
            }
            return true;
            
        }
    }
}
