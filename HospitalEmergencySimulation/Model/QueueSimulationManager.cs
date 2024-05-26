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
        public int NumberOfPatients { get; set; }
        public int TotalArrivalTime { get; set; }
        double TimeTraveled { get; set; }
        bool isCheckInQueue = false;
        public int CurrentSimulationTime { get; set; }
        Boolean isOccuped = false;
        Boolean isCheckPatient = false;
        Distributions distributions = new Distributions();
        List<Patient> HighPriorityQueue = new List<Patient>();
        List<Patient> LowPriorityQueue = new List<Patient>();
        List<Patient> PatientsTreatedHighPriority = new List<Patient>();
        List<Patient> PatientsTreatedLowPriority = new List<Patient>();
        List<Doctor> doctors = new List<Doctor>();
        ObservableCollection<ResultsForTime> resultsForTimes = new ObservableCollection<ResultsForTime>();
        int countPositionRi = 0;
        private double minimumAttentionTimeHighPriority;
        private double maximumAttentionTimeHighPriority;
        private double minimumAttentionTimeLowPriority;
        private double maximumAttentionTimeLowPriority;
        private double lambdaArrivalHighPrority;
        private double lambdaArrivalLowPrority;
        private double numberArrivalIntervals;
        FileHandler fileHandler = new FileHandler();
        List<double> DataRi = new List<double>();
        Random RandomRi = new Random();
        public QueueSimulationManager(double minimumAttentionTimeHighPriority, double maximumAttentionTimeHighPriority, double minimumAttentionTimeLowPriority, double maximumAttentionTimeLowPriority,
                                      double lambdaArrivalHighPrority, double lambdaArrivalLowPrority, int numberArrivalIntervals)
        {
            this.minimumAttentionTimeHighPriority = minimumAttentionTimeHighPriority;
            this.maximumAttentionTimeHighPriority = maximumAttentionTimeHighPriority;
            this.minimumAttentionTimeLowPriority = minimumAttentionTimeLowPriority;
            this.maximumAttentionTimeLowPriority = maximumAttentionTimeLowPriority;
            this.lambdaArrivalHighPrority = lambdaArrivalHighPrority;
            this.lambdaArrivalLowPrority = lambdaArrivalLowPrority;
            this.numberArrivalIntervals = numberArrivalIntervals;
            NumberOfPatients = 0;
            TotalArrivalTime = 5;
            TimeTraveled = 0;
            CurrentSimulationTime = 0;
            createDoctors(3);
            DataRi = fileHandler.ReadCsvFile();
            countPositionRi = RandomRi.Next(0, DataRi.Count);
        }
        public ObservableCollection<ResultsForTime> GetResultForTimes()
        {
            return this.resultsForTimes;
        }
        /* Define el inicio de un tiempo de la simulación, aumentado el tiempo de la misma, así mismo declara el objeto donde se guardará los resultados del tiempo de la simulación
          * genera la cola de prioridad alta y la cola de prioridad baja, a continuación, declara el inicio del servicio con las llegadas o datos existentes
         */
        public void init()
        {
            isCheckInQueue = false;
            CurrentSimulationTime++;
            ResultsForTime resultsForTime = new ResultsForTime();
            resultsForTime.Time = CurrentSimulationTime - 1;
            resultsForTimes.Add(resultsForTime);
            GenerateLowPriorityQueue();
            GenerateHighPriorityQueue(CurrentSimulationTime - 1);
            ManageCustomerService();        
        }
        /*Cuando se termina el tiempo de simulación a analizar, se debe finalizar de atender los pacientes que quedaron en cola
         *para ello se revisa los pacientes de ambas prioridades y se declara un nuevo objeto a guardar los datos del siguiente tiempo de simulación y se realiza el respectivo  servicio
         *retorna 0 si es necesario volver a revisar y 1 si ya todos los pacientes fueron atendidos
        */
        public int FinishAttention()
        {
            if (HighPriorityQueue.Count > 0 || LowPriorityQueue.Count > 0)
            {
                CurrentSimulationTime++;
                ResultsForTime resultsForTime = new ResultsForTime();
                resultsForTime.Time = CurrentSimulationTime - 1;
                resultsForTimes.Add(resultsForTime);
                ManageCustomerService();
                return 0;
            }
            else
            {
                return 1;
            }
        }
        /*
         * Crea los doctores o servidores a emplear en la simulación, para el ejercicio se tienen 3
        */
        private void createDoctors(int NumberOfDoctors)
        {
            for (int i = 0; i < NumberOfDoctors; i++)
            {
                Doctor doctor = new Doctor(i + 1);
                doctors.Add(doctor);
            }
        }

        /*Realiza la creación de un paciente, recibiendo el tiempo de arribo, la prioridad, la lista en la cual se va a ubicar, ya sea de prioridad 
         *alta o baja así mismo recibe el método a ejecutar para asignar el tiempo de servicio.
        */
        private void CreatePatient(double timeArrival, int priority, List<Patient> queue, Action<Patient> AssignSeriviceTime)
        {
            NumberOfPatients++;
            Patient patient = new Patient(NumberOfPatients, priority);
            patient.TimeOfArrival = timeArrival;
            AssignSeriviceTime(patient);
            Patient patientClone = patient.Clone();
            var register = resultsForTimes.FirstOrDefault(r => r.Time == CurrentSimulationTime - 1);
            register.PatientsInSystem.Add(patientClone);
            queue.Add(patient);
        }

        /*Genera la lista de pacientes de alta prioridad, recibe el tiempo de la simulación, también toma el respectivo lambda y lee un Ri a emplear 
         * posteriormente con la trasformada inversa realiza el cálculo de cuantos pacientes llegan para ese instante de tiempo en caso de ser cero
         * se realiza no creo, si es mayor a cero crea el respectivo paciente.
        */
        public void GenerateHighPriorityQueue(int count)
        {
            double Lambda = lambdaArrivalHighPrority;
            double Ri = DataRi[countPositionRi];
            countPositionRi++;
            int PoissonValue = distributions.PoissonInverseTransform(Ri, Lambda);
            if (PoissonValue != 0)
            {
                for (int i = 0; i < PoissonValue; i++)
                {
                    CreatePatient(count, 1, HighPriorityQueue, (patient) => AssignSeriviceTime(patient, minimumAttentionTimeHighPriority, maximumAttentionTimeHighPriority));
                }
            }
        }
        /*Genera la lista de pacientes de baja prioridad, toma el respectivo lambda y lee un Ri a emplear 
        * posteriormente con la trasformada inversa realiza el cálculo del intervalo de llegada de los pacientes en un tiempo dado e
        * y crea pacientes hasta que el tiempo supera el tiempo a estudiar en un momento dado
       */
        public void GenerateLowPriorityQueue()
        {
            int count = 0;
            //Ri para exponencial
            double Ri = DataRi[countPositionRi];
            countPositionRi++;
            //Lamda para expoenencial clientes por unidad de tiempo
            double ArrivalRate = lambdaArrivalLowPrority;
            double ExponentialValue = distributions.ExponentialInverseTransform(Ri, ArrivalRate);

            while (TimeTraveled < CurrentSimulationTime)
            {
                CreatePatient(TimeTraveled, 0, LowPriorityQueue, (patient) => AssignSeriviceTime(patient, minimumAttentionTimeLowPriority, maximumAttentionTimeLowPriority));
                TimeTraveled += ExponentialValue;
            };
        }

        /* Realiza la asignación se tiempos teniendo en cuenta un límite inferior y uno superior para usar la distribución uniforme, esto se realiza mediante
         * la lectura de un Ri dado, asigna un tiempo fijo que no se modificara y un tiempo a descontar
        */
        public void AssignSeriviceTime(Patient patient, double lowerLimit, double highLimit)
        {
            double Ri = DataRi[countPositionRi];
            countPositionRi++;
            double UniformValue = distributions.UniformDistribution(lowerLimit, highLimit, Ri);
            patient.MissingServiceTime = UniformValue;
            patient.ServiceTime = UniformValue;
        }
        /*realiza el proceso de atención a los pacientes, para ello, el doctor revisa si hay un paciente de prioridad alta 
             * si es así lo atiende si no lo hay pasa a revisar los de prioridad baja,
             * así mismo si es liberado busca un nuevo paciente
             */
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
                    bool nextDoctor = CheckQueue(LowPriorityQueue, doctor, "Lower");
                   
                    if (!nextDoctor)
                    {
                        j--;

                    }
                }
            }
        }


        /*recibe la lista a revisar, y la prioridad, con ello primero define un registro para guardar los cambios y 
         * se revisa si el doctor esta libre si es así se atiende al siguiente si no es así se procede a descontar el tiempo 
         * también se verifica si el ya es momento de despachar al paciente, y se agregan los respectivos registros realizados en la iteración
        */

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
                        doctor.IdPatient = patient.IdPatient;
                        patient.IdDoctor = doctor.IdDoctor;
                        doctor.IsOccupied = true;
                        isOccuped = true;
                        Patient patientClone = patient.Clone();
                        register.PatientsInSystem.Add(patientClone);
                        Doctor doctorClone = doctor.Clone();
                        register.Doctors.Add(doctorClone);
                        IsDoctorAttend = true;
                        break;
                    }
                    else if (patient.IdDoctor == doctor.IdDoctor && patient.IsAttended && patient.MissingServiceTime > 0)
                    {
                        if (patient.MissingServiceTime - 1 < 0)
                        {
                            patient.TimeOfExit = (register.Time - 1) + patient.MissingServiceTime;
                        }
                        patient.MissingServiceTime -= 1;
                        isCheckPatient = true;
                        if (patient.MissingServiceTime > 0)
                        {
                            Doctor doctorClone = doctor.Clone();
                            register.Doctors.Add(doctorClone);
                            Patient patientClone = patient.Clone();
                            register.PatientsInSystem.Add(patientClone);
                            IsDoctorAttend = true;
                        }


                    }
                    if (patient.IdDoctor == doctor.IdDoctor && patient.IsAttended && patient.MissingServiceTime <= 0)
                    {
                        doctor.IsOccupied = false;
                        if (priority == "High")
                        {
                            patient.FinishedAttended = true;
                            double timeW = (patient.TimeOfExit - (patient.TimeOfArrival + patient.ServiceTime)<0)? 0 : patient.TimeOfExit - (patient.TimeOfArrival + patient.ServiceTime);
                            patient.TimeWait = timeW;
                            PatientsTreatedHighPriority.Add(patient);
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
                            patient.FinishedAttended = true;
                            double timeW = (patient.TimeOfExit - (patient.TimeOfArrival + patient.ServiceTime) < 0) ? 0 : patient.TimeOfExit - (patient.TimeOfArrival + patient.ServiceTime);
                            patient.TimeWait = timeW;
                            PatientsTreatedLowPriority.Add(patient);
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
