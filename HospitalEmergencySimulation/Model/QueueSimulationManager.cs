using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HospitalEmergencySimulation.Model
{
    internal class QueueSimulationManager
    {
        public int NumberOfPatients {  get; set; }
        public int TotalArrivalTime {  get; set; }
        double TimeTraveled {  get; set; }
        bool isCheckInQueue = false;
        public int CurrentSimulationTime {  get; set; }
        Boolean isOccuped = false;
        Boolean isCheckPatient = false;
        Distributions distributions = new Distributions();
        List<Patient> HighPriorityQueue = new List<Patient>();
        List<Patient> LowPriorityQueue = new List<Patient>();
        List<Patient> PatientsTreatedHighPriority = new List<Patient>();
        List<Patient> PatientsTreatedLowPriority = new List<Patient>();
        List<Doctor> doctors = new List<Doctor>();
        ObservableCollection<ResultsForTime> resultsForTimes = new ObservableCollection<ResultsForTime>();

        private int minimumAttentionTimeHighPriority;
        private int maximumAttentionTimeHighPriority;
        private int minimumAttentionTimeLowPriority;
        private int maximumAttentionTimeLowPriority;
        private int lambdaArrivalHighPrority;
        private int lambdaArrivalLowPrority;
        private int numberArrivalIntervals;
        public QueueSimulationManager(int minimumAttentionTimeHighPriority, int maximumAttentionTimeHighPriority, int minimumAttentionTimeLowPriority, int maximumAttentionTimeLowPriority,
                                      int lambdaArrivalHighPrority, int lambdaArrivalLowPrority, int numberArrivalIntervals) 
        {
            this.minimumAttentionTimeHighPriority = minimumAttentionTimeHighPriority;
            this.maximumAttentionTimeHighPriority=maximumAttentionTimeHighPriority;
            this.minimumAttentionTimeLowPriority = minimumAttentionTimeLowPriority;
            this.maximumAttentionTimeLowPriority=maximumAttentionTimeLowPriority;
            this.lambdaArrivalHighPrority = lambdaArrivalHighPrority;
            this.lambdaArrivalLowPrority = lambdaArrivalLowPrority;
            this.numberArrivalIntervals = numberArrivalIntervals;
            NumberOfPatients =0;
            TotalArrivalTime = 5;
            TimeTraveled = 0;
            CurrentSimulationTime = 0;
            createDoctors(3);
           // init();
        }
        public ObservableCollection<ResultsForTime> GetResultForTimes()
        {
            return this.resultsForTimes;
        }

        public void init()
        {
                isCheckInQueue = false;
            CurrentSimulationTime++;
            ResultsForTime resultsForTime = new ResultsForTime();
                resultsForTime.Time = CurrentSimulationTime-1;
                resultsForTimes.Add(resultsForTime);
                GenerateLowPriorityQueue();
                GenerateHighPriorityQueue(CurrentSimulationTime);
               // MessageBox.Show("hay en alta " + HighPriorityQueue.Count);
                //MessageBox.Show("hay en baja " + LowPriorityQueue.Count);
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
        public int FinishAttention ()
        {
            if(HighPriorityQueue.Count > 0 || LowPriorityQueue.Count > 0)
            {
                CurrentSimulationTime++;
                ResultsForTime resultsForTime = new ResultsForTime();
                resultsForTime.Time = CurrentSimulationTime - 1;
                resultsForTimes.Add(resultsForTime);
                ManageCustomerService();
                
                /*Console.WriteLine(CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime + CurrentSimulationTime + "" + CurrentSimulationTime);
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
                }*/
                return 0;
            }
            else
            {
                return 1;
            }
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
            Console.WriteLine("nuemro pacientes "+ NumberOfPatients);
            Patient patient = new Patient(NumberOfPatients, priority);
            patient.TimeOfArrival = timeArrival;
            AssignSeriviceTime(patient);
            Patient patientClone = patient.Clone();
            var register = resultsForTimes.FirstOrDefault(r => r.Time == CurrentSimulationTime-1);
            register.PatientsInSystem.Add(patientClone);
            Console.WriteLine("Cantidsad " + register.PatientsInSystem.Count + "id paciente "+ patient.IdPatient );

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
                    CreatePatient(count, 1, HighPriorityQueue, (patient) => AssignSeriviceTime(patient, 3, 5));
                }
            }
            Console.WriteLine("ddddddddddddddddddddddddd " + HighPriorityQueue.Count);
        }
        public void GenerateLowPriorityQueue()
        {
            int count = 0;
            //Ri para exponencial
            double Ri = new Random().NextDouble();
            //Lamda para expoenencial clientes por unidad de tiempo
            double ArrivalRate = 0.8;
            double ExponentialValue = distributions.ExponentialInverseTransform(Ri, ArrivalRate);

            while (TimeTraveled < CurrentSimulationTime)
            {
                CreatePatient(TimeTraveled, 0, LowPriorityQueue, (patient) => AssignSeriviceTime(patient, 1, 2));
                TimeTraveled += ExponentialValue;
                Console.WriteLine("cccccccccccccccccccccccccc " + LowPriorityQueue.Count);
               
            };
            
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
               /* Doctor doctorClone = doctor.Clone();
                var register = resultsForTimes.FirstOrDefault(r => r.Time == CurrentSimulationTime - 1);
                register.Doctors.Add(doctorClone);*/
            }
        }
            
        
       
        public bool CheckQueue(List<Patient> queue, Doctor doctor, String priority)
        {
            bool IsCheck = false;
            bool IsDoctorAttend = false;
            if (queue.Count > 0)
            {
                var register = resultsForTimes.FirstOrDefault(r => r.Time == CurrentSimulationTime - 1);
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
                        // register.PatientsInSystem.FirstOrDefault(c => c.IdPatient == patient.IdPatient).IdDoctor = patient.IdDoctor;
                        // register.PatientsInSystem.FirstOrDefault(c => c.IdPatient == patient.IdPatient).IsAttended = patient.IsAttended;
                        Patient patientClone = patient.Clone();
                        register.PatientsInSystem.Add(patientClone);
                        Doctor doctorClone = doctor.Clone();
                        register.Doctors.Add(doctorClone);
                        IsDoctorAttend = true;
                        break;
                    }
                    else if (patient.IdDoctor == doctor.IdDoctor && patient.IsAttended && patient.MissingServiceTime > 0)
                    {
                        if(patient.IdPatient == 2)
                        {
                            MessageBox.Show("tinempo actual " + CurrentSimulationTime + " tiempo queda " + patient.MissingServiceTime);
                        }
                        if (patient.MissingServiceTime - 1 < 0)
                        {
                            patient.TimeOfExit = (register.Time-1)+ patient.MissingServiceTime;
                        }
                        patient.MissingServiceTime -= 1;
                        isCheckPatient = true;
                        //int df = register.PatientsInSystem[0].IdPatient;
                        //Console.WriteLine("Que es register "+register.Time + " cual es register " + df + " que es paciente " + patient.IdPatient + "cantidaad " +register.PatientsInSystem.Count);
                        //register.PatientsInSystem.FirstOrDefault(c => c.IdPatient == patient.IdPatient).MissingServiceTime = patient.MissingServiceTime;
                        if (patient.MissingServiceTime > 0)
                        {
                            Doctor doctorClone = doctor.Clone();
                            register.Doctors.Add(doctorClone);
                            Patient patientClone = patient.Clone();
                            register.PatientsInSystem.Add(patientClone);
                            IsDoctorAttend = true;
                            //IsCheck = true;
                        }


                    }
                    if (patient.IdDoctor == doctor.IdDoctor && patient.IsAttended && patient.MissingServiceTime <= 0)
                    {
                        doctor.IsOccupied = false;
                        //(priority == "High" ? PatientsTreatedHighPriority : PatientsTreatedLowPriority).Add(patient);
                        if (priority == "High")
                        {
                            Console.WriteLine("************************************************************");
                            patient.FinishedAttended = true;
                            patient.TimeWait = patient.TimeOfExit - (patient.TimeOfArrival + patient.ServiceTime);
                            PatientsTreatedHighPriority.Add(patient);
                            //register.PatientsInSystem.FirstOrDefault(c => c.IdPatient == patient.IdPatient).FinishedAttended = true;
                            Patient patientClone = patient.Clone();
                            patientClone.MissingServiceTime = 0;
                            register.PatientsInSystem.Add(patientClone);
                            HighPriorityQueue.Remove(patient);
                            Doctor doctorClone = doctor.Clone();
                            IsDoctorAttend = true;
                            register.Doctors.Add(doctorClone);
                            CheckQueue(queue, doctor, priority);
                        }
                        else
                        {
                            Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
                            patient.FinishedAttended = true;
                            patient.TimeWait = patient.TimeOfExit - (patient.TimeOfArrival + patient.ServiceTime);
                            PatientsTreatedLowPriority.Add(patient);
                            //register.PatientsInSystem.FirstOrDefault(c => c.IdPatient == patient.IdPatient).FinishedAttended = true;
                            Patient patientClone = patient.Clone();
                            patientClone.MissingServiceTime = 0;
                            register.PatientsInSystem.Add(patientClone);
                            LowPriorityQueue.Remove(patient);
                            Doctor doctorClone = doctor.Clone();
                            IsDoctorAttend = true;
                            register.Doctors.Add(doctorClone);
                            CheckQueue(queue, doctor, priority);
                        }
                        
                        IsCheck = true;
                        break;
                    }
                    if (isCheckPatient)
                    {
                        break;
                    }
                }
                if (!IsDoctorAttend && !isCheckInQueue)
                {
                    isCheckInQueue = true;
                    Doctor doctorClone = doctor.Clone();
                    register.Doctors.Add(doctorClone);
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
