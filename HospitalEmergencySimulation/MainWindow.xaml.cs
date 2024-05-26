using HospitalEmergencySimulation.Controller;
using HospitalEmergencySimulation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace HospitalEmergencySimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private DispatcherTimer timer;
        List<Tuple<double, double>> coordinateHighPriority = new List<Tuple<double, double>>();
        List<Tuple<double, double>> coordinateLowPriority = new List<Tuple<double, double>>();
        ObservableCollection<ResultsForTime> resultsForTime = new ObservableCollection<ResultsForTime>();
        List<FormatPatient> formatPatientsHighPriority = new List<FormatPatient>();
        List<FormatPatient> formatPatientsLowPriorty = new List<FormatPatient>();
        List<FormatPatient> formatPatienttAll = new List<FormatPatient>();
        List<FormarDataDoctor> DataDoctor = new List<FormarDataDoctor>();
        Dictionary<int, FormatServers> servers = new Dictionary<int, FormatServers>();
        private int count = 0;
        private int  countDelete = 0;
        int countHiPriority = 0;
        int countLowPriority = 0;
        int gdf = 177;
        private int patientHighCount = 0;
        private int patientLowCount = 0;
        private System.Windows.Controls.Image lastImage = null;
        private System.Windows.Controls.Image lastImageLowPriority = null;
        private System.Windows.Controls.Image lastPatient = null;
        int positionInitialLowPriority = 242;
        int QuantityPatientientsHigh = 0;
        int QuantityPatientientsLow = 0;
        int countHigh = 0;
        int countLow = 0;
        int countServersAvailable = 0;
        Boolean IsMovementHigh = false;
        Boolean IsMovementLow = false;
        HashSet<int> patientsIdHigh = new HashSet<int>();
        HashSet<int> patientsIdLow = new HashSet<int>();
        HashSet<int> Quantity = new HashSet<int>();
        ControllerSimulation controller;
        public MainWindow(ControllerSimulation controller)
        {
            this.controller = controller;
            InitializeComponent();
            coordinateHighPriority.Add(new Tuple<double, double>(0, 300));
            coordinateLowPriority.Add(new Tuple<double, double>(0, 300));
            Fondo.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título.png"));
            ServerOne.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\file (1).png"));
            ServerTwo.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\file (2).png"));
            ServerThree.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\file (3).png"));      
            ca.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (4).png"));      
            ca2.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (4).png"));      
            camino.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (4).png"));      
            camino2.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (5).png"));      
            camino22.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (5).png"));      
            camino222.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (5).png"));      
            camino3.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (5).png"));      
            camino33.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (5).png"));      
            camino333.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\Sin título (5).png"));      
            resultsForTime = controller.GetResults();
            servers[1] = new FormatServers
            {
                Coordinate = 148,
                IsOcupped = false,
            };
            servers[2] = new FormatServers
            {
                Coordinate = 380,
                IsOcupped = false,
            };
            servers[3] = new FormatServers
            {
                Coordinate = 590,
                IsOcupped = false,
            };
            foreach (ResultsForTime r in resultsForTime)
            {
                Quantity.Add(r.Time);
            }

        }
        public void loadDataForTime(int count)
        {
            formatPatienttAll = new List<FormatPatient>();
            foreach (ResultsForTime result in resultsForTime)
            {
                foreach (Patient result2 in result.PatientsInSystem)
                {
                    if (result.Time == count)
                    {
                        FormatPatient aux = new FormatPatient
                        {
                            TimeSimulation = result.Time,
                            IdDoctor = result2.IdDoctor,
                            IdPatient = result2.IdPatient,
                            IsAttended = result2.IsAttended,
                            ServiceTime = result2.ServiceTime,
                            MissingServiceTime = result2.MissingServiceTime,
                            TimeOfArrival = result2.TimeOfArrival,
                            TimeOfExit = result2.TimeOfExit,
                            FinishedAttended = result2.FinishedAttended,
                            Priority = result2.Priority,
                            TimeWait = result2.TimeWait,
                        };

                        if (aux.Priority == 1)
                        {
                            formatPatientsHighPriority.Add(aux);
                            patientsIdHigh.Add(aux.IdPatient);
                        }
                        else if (aux.Priority == 0)
                        {
                            formatPatientsLowPriorty.Add(aux);
                            patientsIdLow.Add(aux.IdPatient);
                        }
                        formatPatienttAll.Add(aux);

                    }
                }
                foreach (Doctor result2 in result.Doctors)
                {
                    FormarDataDoctor aux = new FormarDataDoctor
                    {
                        TimeSimulation = result.Time,
                        IdDoctor = result2.IdDoctor,
                        IdPatient = result2.IdPatient,
                        IsOccupied = result2.IsOccupied,
                        Time = result2.Time
                    };
                    DataDoctor.Add(aux);
                }
            }
        }
        public async void StartSimulation(object sender, RoutedEventArgs e)
        {
            while (true)
            {
               
                    myLabe.Content = "hay tiempo " + Quantity.Count;
                    numerbucle.Content = "Está en número " + count;
                    loadDataForTime(count);

                    QuantityPatientientsHigh = patientsIdHigh.Count;
                    QuantityPatientientsLow = patientsIdLow.Count;

                    myLabel.Content = "Hay Alta " + formatPatientsHighPriority.Count + " Hay Baja " + formatPatientsLowPriorty.Count;

                    await ExecuteSimulationStep();
                        count++;
                        if (count > Quantity.Count)
                        {
                            break;
                        }

            }
        }
        private async Task countServerDesocupped()
        {
            foreach (KeyValuePair<int, FormatServers> item in servers)
            {
                if (!item.Value.IsOcupped)
                {
                    countServersAvailable++;
                }
            }
        }

        private async Task ExecuteSimulationStep()
        {
            // Crear pacientes y esperar 3.5 segundos
            await CreatePatientAsync(2.5);
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Iniciar el servicio y esperar tiempo adecuado
            await StartServiceAsync();
            await Task.Delay(TimeSpan.FromSeconds(1)); // Ajusta este valor según tus necesidades

            // Terminar el servicio del paciente
            await TerminateServicePatienteAsync();
            await Task.Delay(TimeSpan.FromSeconds(3.5));
            cantidadPendiente.Content = "se deben atender " + countDelete;
            if (countDelete > 0)
            {
                await atendNew(countDelete);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }

            await countServerDesocupped();
            cantidadPendiente.Content = "quedan libres  " + countServersAvailable;
            if (countServersAvailable > 0)
            {
               await atendNew2(countServersAvailable);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            
        }

        private async Task atendNew(int value)
        {
            int aux = 0;
            while(aux < value)
            {
                await StartServiceAsync();
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                aux++;
            }
            countDelete =0;
        }
        private async Task atendNew2(int value)
        {
            int aux = 0;
            while (aux < value)
            {
                await StartServiceAsync();
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                aux++;
            }
            countServersAvailable = 0;
        }

        private async Task CreatePatientAsync(double interval)
        {
            // Aquí puedes poner la lógica de creación de pacientes
            CreatePatient(interval); // Asumiendo que CreatePatient es sincrónico
            await Task.Delay(TimeSpan.FromSeconds(interval)); // Esperar el intervalo
        }

        private async Task StartServiceAsync()
        {
            // Filtrar la lista para obtener solo los pacientes que están siendo atendidos
            // Ordenar la lista por IdPatient
            List<FormatPatient> orderedPatients = formatPatienttAll
                .OrderByDescending(p => p.Priority) // Ordenar primero por prioridad 
                .ThenBy(p => p.TimeOfArrival) // Luego ordenar por tiempo de llegada
                .ToList();

            int coundf = 0;
            

            foreach (FormatPatient patient in orderedPatients)
            {
                if (!patient.IsAttended)
                {
                    foreach (KeyValuePair<int, FormatServers> item in servers)
                    {
                        bool isChanged = false;

                        if (!item.Value.IsOcupped && patient.Priority == 1)
                        {
                            attend("HighPriority", item.Value.Coordinate);
                            item.Value.IsOcupped = true;
                            item.Value.IdPatient = patient.IdPatient;
                            this.IsMovementHigh = true;
                            countHigh++;
                            isChanged = true;
                        }
                        else if (!item.Value.IsOcupped && patient.Priority == 0)
                        {
                            countLow++;
                            coundf++;
                            this.IsMovementLow = true;
                            attend("LowPriority", item.Value.Coordinate);
                            item.Value.IdPatient = patient.IdPatient;
                            item.Value.IsOcupped = true;
                            await Task.Delay(TimeSpan.FromSeconds(4));
                            isChanged = true;
                        }

                        if (isChanged)
                        {
                            MoveRow();
                            myLabe.Content = $"Paciente movido, Id: {patient.IdPatient}, Prioridad: {patient.Priority}";
                            await Task.Delay(TimeSpan.FromSeconds( 2));
                            break; // Salir del loop de servidores para procesar el siguiente paciente
                        }
                    }
                }
            }
        }


        private async Task TerminateServicePatienteAsync()
        {
            await TerminateServicePatiente(); // Llamar al método asincrónico
        }

        private async Task TerminateServicePatiente()
        {
            Dictionary<double, string> end = new Dictionary<double, string>();
            List<FormatPatient> patientsBeingAttended = formatPatienttAll.Where(p => p.IsAttended == true).ToList();

            foreach (FormatPatient patient in patientsBeingAttended)
            {
                string namePriorityPatient = (patient.Priority == 1) ? "AHighPriority" : "ALowPriority";
                
                if (patient.MissingServiceTime <= 0)
                {
                    foreach(KeyValuePair<int, FormatServers> item in servers){
                        if (item.Value.IdPatient == patient.IdPatient)
                        {
                            int serverPatient = item.Key;
                            end[serverPatient] = namePriorityPatient;
                        }
                    }
                   
                }
            }

            foreach (KeyValuePair<double, string> item in end)
            {
                string namePriority = item.Value;
                double serverr = item.Key;
                int s = (int)serverr;
                FormatServers valor = servers[s];
                double server = valor.Coordinate;
                System.Windows.Controls.Image patientDischarged = null;
                double x = 0;
                double y = 0;

                foreach (UIElement element in canvas.Children)
                {
                    if (element is System.Windows.Controls.Image image && image.Name.StartsWith(namePriority))
                    {
                        x = Canvas.GetLeft(image);
                        y = Canvas.GetTop(image);
                        if (x == server && y == 103)
                        {
                            patientDischarged = image;
                            break;
                        }
                    }
                }

                if (patientDischarged != null)
                {
                    DoubleAnimation moverX = new DoubleAnimation
                    {
                        From = x,
                        To = server - 94, // Ajustar según la posición deseada
                        Duration = TimeSpan.FromSeconds(0.5)
                    };

                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

                    moverX.Completed += (s, e) =>
                    {
                        DoubleAnimation moverY = new DoubleAnimation
                        {
                            From = y,
                            To = -30, // Ajustar según la posición deseada
                            Duration = TimeSpan.FromSeconds(0.5)
                        };

                        moverY.Completed += async (s2, e2) =>
                        {
                            countDelete++;
                            canvas.Children.Remove(patientDischarged);
                            FormatServers d = servers[(int)item.Key];
                            d.IsOcupped = false;
                            tcs.SetResult(true);
                        };

                        patientDischarged.BeginAnimation(Canvas.TopProperty, moverY);
                    };

                    patientDischarged.BeginAnimation(Canvas.LeftProperty, moverX);
                    await tcs.Task; // Esperar a que la animación se complete
                }
            }

          //  StartService(); // Llamar a StartService después de que todos los pacientes hayan sido dados de alta
        }

      

        
        public void CreatePatient(double seconds)
        {
            /*// Cambia esto al número de segundos que quieres esperar
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(seconds) };
           // 
            timer.Tick += Timer_Tick;
            timer.Start();*/

            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(seconds);
                if (patientsIdHigh.Count > 0)
                {
                    timer.Tick += Timer_Tick;
                }
                else if (patientsIdLow.Count > 0)
                {
                    timer.Tick += Timer_Low_prority;
                }
            }

            // Inicia el temporizador
            timer.Start();
        }
        /* private void myButton_Click(object sender, RoutedEventArgs e)
         {
             CreatePatient(3.5);
         }*/
        private async void StartService()
        {
            // Filtrar la lista para obtener solo los pacientes que están siendo atendidos
            // Ordenar la lista por IdPatient
            List<FormatPatient> orderedPatients = formatPatienttAll
                .OrderByDescending(p => p.Priority) // Ordenar primero por prioridad 
                .ThenBy(p => p.TimeOfArrival) // Luego ordenar por tiempo de llegada
                .ToList();
            
            int coundf = 0;
            
            
            foreach (FormatPatient patient in orderedPatients)
            {

                myLabe.Content = "entro deberia mover " + 44;
                if (!patient.IsAttended)
                {
                    foreach (KeyValuePair<int, FormatServers> item in servers)
                    {

                        bool isChanged = false;
                        //MessageBox.Show("esta ocupado server " + item.Key +" "+ item.Value.IsOcupped + " prioridad " + patient.Priority);
                        if (!item.Value.IsOcupped && patient.Priority == 1)
                        {
                            attend("HighPriority", item.Value.Coordinate);
                            item.Value.IsOcupped = true;
                            item.Value.IdPatient = patient.IdPatient;
                            this.IsMovementHigh = true;
                            countHigh++;
                            isChanged = true;
                        }
                        else if (!item.Value.IsOcupped && patient.Priority == 0)
                        {
                            // MessageBox.Show("id paciente " + patient.IdPatient + " entro a  mover baja");
                            countLow++;
                            coundf++;
                            myLabe.Content = "entro deberia mover " + coundf;
                            this.IsMovementLow = true;
                            attend("LowPriority", item.Value.Coordinate);
                            item.Value.IdPatient = patient.IdPatient;
                            item.Value.IsOcupped = true;
                            isChanged = true;
                        }
                        if (isChanged)
                        {
                            MoveRow();
                            await Task.Delay(TimeSpan.FromSeconds(3.5 + 2));
                            
                        }

                    }

                }

            }


        }

        public void MoveRow()
        {
            if (IsMovementLow)
            {
                AdvanceQueue("LowPriority", 749);
                IsMovementLow = false; // Resetear el indicador de movimiento
            }
            if (IsMovementHigh)
            {
                AdvanceQueue("HighPriority", 691);
                IsMovementHigh = false; // Resetear el indicador de movimiento
            }
        }

        public async void AdvanceQueue(string namePriority, int position)
        {
            List<Task> animationTasks = new List<Task>();

            foreach (UIElement element in canvas.Children)
            {
                if (element is System.Windows.Controls.Image image && image.Name.StartsWith(namePriority))
                {
                    // Crear una tarea de finalización para esperar la animación
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

                    if (Canvas.GetTop(image) != 128)
                    {
                        if (Canvas.GetLeft(image) >= position)
                        {
                            DoubleAnimation moverYY = new DoubleAnimation
                            {
                                To = Canvas.GetTop(image) - 30,
                                Duration = TimeSpan.FromSeconds(1)
                            };

                            moverYY.Completed += (s, e) => tcs.SetResult(true);
                            image.BeginAnimation(Canvas.TopProperty, moverYY);
                        }
                        else if ((Canvas.GetTop(image) == 177 && !image.Name.StartsWith(namePriority)) ||
                                 (Canvas.GetTop(image) == 242 && Canvas.GetTop(image) != 749 && image.Name.StartsWith("LowPriority")) ||
                                 (image.Name.StartsWith("LowPriority") && Canvas.GetTop(image) != 177))
                        {
                            DoubleAnimation moverX = new DoubleAnimation
                            {
                                From = Canvas.GetLeft(image),
                                To = Canvas.GetLeft(image) + 30,
                                Duration = TimeSpan.FromSeconds(1)
                            };

                            moverX.Completed += (s, e) => tcs.SetResult(true);
                            image.BeginAnimation(Canvas.LeftProperty, moverX);
                        }
                        else
                        {
                            DoubleAnimation moverY = new DoubleAnimation
                            {
                                From = Canvas.GetTop(image),
                                To = Canvas.GetTop(image) - 30,
                                Duration = TimeSpan.FromSeconds(1)
                            };

                            moverY.Completed += (s, e) => tcs.SetResult(true);
                            image.BeginAnimation(Canvas.TopProperty, moverY);
                        }

                        // Agregar la tarea de animación a la lista
                        animationTasks.Add(tcs.Task);
                    }
                }
            }

            // Esperar a que todas las animaciones terminen
            await Task.WhenAll(animationTasks);
        }
        private void InitService(object sender, RoutedEventArgs e)
        {
            StartService();
            // attend("LowPriority", 148); // 590, 380, 148
            //  AdvanceQueue("LowPriority");
        }

        private void TerminateService(object sender, RoutedEventArgs e)
        {
            // TerminateServicePatiente("LowPriority", 148);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {

            // Crear una nueva image y agregarla al canvas
            var image = new System.Windows.Controls.Image
            {
                Name = "HighPriority" + (coordinateLowPriority.Count + 1),
                Width = 30,
                Height = 30,
                Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\file.png")) // Cambia 'TuImagen.jpg' por el nombre de tu image
            };
            
            canvas.Children.Add(image);
            double initialTop = canvas.ActualHeight - image.Height; // Cambia '50' por la altura de tus imágenes
            Canvas.SetTop(image, initialTop); // Establecer la posición inicial de la image
            Canvas.SetLeft(image, 10);
            // Manejar el evento Loaded para asegurarse de que la image esté completamente colocada antes de iniciar la animación
            image.Loaded += (s, _) =>
            {
                // Animación de aparición
                DoubleAnimation aparecer = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                image.BeginAnimation(UIElement.OpacityProperty, aparecer);

                // Obtener la posición inicial de la image después de que esté completamente colocada
                double x = Canvas.GetLeft(image);
                double y = Canvas.GetTop(image);
                //coordinateHighPriority.Add(new Tuple<double, double>(x, y)); // Guardar las coordinateHighPriority de la image
                double lastPosition = coordinateHighPriority[coordinateHighPriority.Count - 1].Item2;
                // myLabel.Content = "Nuevo contenido  " + lastPosition + " CANTIDAD "+ coordinateHighPriority.Count ;

                // Animación de movimiento
                DoubleAnimation moverY = new DoubleAnimation
                {
                    From = y,
                    To = gdf,// lastPosition, // Cambia esto a la posición Y donde quieres que se quede la image
                    Duration = TimeSpan.FromSeconds(0.3)
                };
                coordinateHighPriority.Add(new Tuple<double, double>(0, coordinateHighPriority[coordinateHighPriority.Count - 1].Item2 - 30));

                moverY.Completed += (s, e) =>
                {
                    Boolean isFirst = false;
                    double positionFinal = 0;
                    int count = 0;
                    foreach (UIElement element in canvas.Children)
                    {
                        if (element is System.Windows.Controls.Image image && image.Name.StartsWith("HighPriority"))
                        {

                            count++;
                            if (count > 1)
                            {
                                isFirst = true;
                            }
                        }
                    }
                    if (canvas.Children.Count > 0)
                    {
                        if (lastImage != null && image != null)
                        {
                            double xc = Canvas.GetLeft(lastImage);
                            double yc = Canvas.GetTop(image);
                            myLabel.Content = "Posicion X: " + xc + " prueba: " + isFirst + " yy: " + yc;
                        }
                    }

                    positionFinal = isFirst ? Canvas.GetLeft(lastImage) - 30 : 691;
                    if (positionFinal > 15)
                    {
                        // Animación de movimiento horizontal
                        DoubleAnimation moverX = new DoubleAnimation
                        {


                            To = positionFinal,
                            Duration = TimeSpan.FromSeconds(1)

                        };
                        moverX.Completed += (s, g) =>
                        {
                            lastImage = image;
                        };
                        image.BeginAnimation(Canvas.LeftProperty, moverX);
                    }
                    else
                    {
                        gdf += 30;
                    }

                };

                image.BeginAnimation(Canvas.TopProperty, moverY);


                //myLabel.Content = "Nuevo contenido  " + lastPosition + " CANTIDAD " + coordinateHighPriority.Count;
            };
            // Incrementa el contador de pacientes
            patientHighCount++;

            // Si ya se han creado todos los pacientes, detén el temporizador
            if (patientHighCount >= QuantityPatientientsHigh)
            {
                timer.Stop();
            }
        }

  


        public async void attend(string namePriority, double server)
        {
            System.Windows.Controls.Image firstElement = null;
            foreach (UIElement element in canvas.Children)
            {
                if (element is System.Windows.Controls.Image image && image.Name.StartsWith(namePriority))
                {
                    firstElement = image;
                    break;
                }
            }
            if (firstElement != null)
            {
                // Aquí tienes el primer elemento de la fila1, ahora puedes moverlo
                double x = Canvas.GetLeft(firstElement);
                double y = Canvas.GetTop(firstElement);

                // Animación de movimiento
                DoubleAnimation mover = new DoubleAnimation
                {
                    From = y,
                    To = 128, // Cambia esto a la posición Y donde quieres que se quede la imagen
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                mover.Completed += (s, e) =>
                {
                    DoubleAnimation moverX = new DoubleAnimation
                    {

                        From = x,
                        To = server,//590,
                        Duration = TimeSpan.FromSeconds(0.5)

                    };
                    moverX.Completed += (s, e) =>
                    {
                        double yy = Canvas.GetTop(firstElement);
                        DoubleAnimation moverY = new DoubleAnimation
                        {

                            From = yy,
                            To = 103,
                            Duration = TimeSpan.FromSeconds(0.5)

                        };
                        firstElement.Name = "A" + namePriority;
                        firstElement.BeginAnimation(Canvas.TopProperty, moverY);
                    };
                    firstElement.BeginAnimation(Canvas.LeftProperty, moverX);

                    // Aquí la animación ha terminado, por lo que puedes obtener las coordenadas finales
                    double finalX = Canvas.GetLeft(firstElement);
                    double finalY = Canvas.GetTop(firstElement);
                    // Ahora puedes guardar las coordenadas finales
                    coordinateHighPriority.Add(new Tuple<double, double>(finalX, finalY));
                };

                firstElement.BeginAnimation(Canvas.TopProperty, mover);
                await Task.Delay(TimeSpan.FromSeconds(1.2));
            }
        }












        private void Timer_Low_prority(object sender, EventArgs e)
        {

            // Crear una nueva image y agregarla al canvas
            var image = new System.Windows.Controls.Image
            {
                Name = "LowPriority" + (coordinateLowPriority.Count + 1),
                Width = 30,
                Height = 30,
                Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\abuelo.png")) // Cambia 'TuImagen.jpg' por el nombre de tu image
            };
            canvas.Children.Add(image);
            double initialTop = canvas.ActualHeight - image.Height;// Cambia '50' por la altura de tus imágenes
            Canvas.SetTop(image, initialTop); // Establecer la posición inicial de la image

            double initialLeft = 100; // Cambia '100' por la posición X donde quieres que aparezca la imagen
            Canvas.SetLeft(image, initialLeft);
            // Manejar el evento Loaded para asegurarse de que la image esté completamente colocada antes de iniciar la animación
            image.Loaded += (s, _) =>
            {
                // Animación de aparición
                DoubleAnimation aparecer = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                image.BeginAnimation(UIElement.OpacityProperty, aparecer);

                // Obtener la posición inicial de la image después de que esté completamente colocada
                double x = Canvas.GetLeft(image);
                double y = Canvas.GetTop(image);
                //coordinateHighPriority.Add(new Tuple<double, double>(x, y)); // Guardar las coordinateHighPriority de la image
                double lastPosition = coordinateLowPriority[coordinateLowPriority.Count - 1].Item2;
                //  myLabel.Content = "Nuevo contenido  " + lastPosition + " CANTIDAD " + coordinateHighPriority.Count;

                // Animación de movimiento
                DoubleAnimation moverY = new DoubleAnimation
                {
                    From = initialTop,
                    To = positionInitialLowPriority,//lastPosition, // Cambia esto a la posición Y donde quieres que se quede la image
                    Duration = TimeSpan.FromSeconds(0.2)
                };
                moverY.Completed += (s, e) =>
                {
                    Boolean isFirst = false;
                    double positionFinal = 0;
                    int count = 0;
                    foreach (UIElement element in canvas.Children)
                    {
                        if (element is System.Windows.Controls.Image image && image.Name.StartsWith("LowPriority"))
                        {

                            count++;
                            if (count > 3)
                            {
                                isFirst = true;
                            }
                        }
                    }
                    double positionYFinal = 230;
                    if (canvas.Children.Count > 0)
                    {
                        if (lastImageLowPriority != null && image != null)
                        {
                            positionYFinal = Canvas.GetTop(lastImageLowPriority);
                            double xc = Canvas.GetLeft(lastImageLowPriority);
                            double yc = Canvas.GetTop(image);

                        }
                    }

                    positionFinal = isFirst ? Canvas.GetLeft(lastImageLowPriority) - 30 : 749;
                    if (positionFinal > 100)
                    {
                        myLabel.Content = "Posvicion X: " + positionFinal + " prueba: " + isFirst + " yy: ";
                        // Animación de movimiento horizontal
                        DoubleAnimation moverX = new DoubleAnimation
                        {

                            From = x,
                            To = positionFinal,
                            Duration = TimeSpan.FromSeconds(0.5)

                        };
                        if (isFirst)
                        {
                            moverX.Completed += (s, h) =>
                            {
                                lastImageLowPriority = image;

                            };
                        }
                        moverX.Completed += (s, g) =>
                        {
                            Boolean isFirst = false;
                            double positionFinal = 0;
                            int count = 0;
                            foreach (UIElement element in canvas.Children)
                            {
                                if (element is System.Windows.Controls.Image image && image.Name.StartsWith("LowPriority"))
                                {

                                    count++;
                                    if (count > 1)
                                    {
                                        isFirst = true;
                                    }
                                }
                            }
                            if (canvas.Children.Count > 0)
                            {
                                if (lastImageLowPriority != null && image != null)
                                {
                                    double xc = Canvas.GetLeft(lastImageLowPriority);
                                    double yc = Canvas.GetTop(image);
                                    myLabel.Content = "Posicionc X: " + xc + " prueba: " + isFirst + " yy: " + yc;
                                }
                            }
                            myLabel.Content = "Posicion Y: " + positionFinal + " prueba: " + isFirst + " yy: ";
                            positionFinal = isFirst ? Canvas.GetTop(lastImageLowPriority) + 30 : 177;
                            if (positionFinal <= 230)
                            {

                                myLabel.Content = "Posicion Y: " + positionFinal + " prueba: " + isFirst + " yy: ";
                                DoubleAnimation moverYY = new DoubleAnimation
                                {

                                    To = positionFinal,//lastPosition, // Cambia esto a la posición Y donde quieres que se quede la image
                                    Duration = TimeSpan.FromSeconds(0.2)
                                };
                                moverYY.Completed += (s, h) =>
                                {
                                    lastImageLowPriority = image;

                                };
                                image.BeginAnimation(Canvas.TopProperty, moverYY);
                            }



                        };
                        image.BeginAnimation(Canvas.LeftProperty, moverX);
                    }
                    else if (positionFinal < 100)
                    {
                        myLabel.Content = "Possaicion Y: " + positionFinal + " prueba: " + isFirst + " yy: ";
                        positionInitialLowPriority += 30;
                    }
                };
                image.BeginAnimation(Canvas.TopProperty, moverY);
                //myLabel.Content = "Nuevo contenido  " + lastPosition + " CANTIDAD " + coordinateHighPriority.Count;
            };
            // Incrementa el contador de pacientes
            patientLowCount++;
            // Si ya se han creado todos los pacientes, detén el temporizador
            if (patientLowCount >= QuantityPatientientsLow)
            {
                timer.Stop();
            }
        }

        public void ViewTables(object sender, RoutedEventArgs e)
        {

            myLabe.Content = "envio " + resultsForTime.Count;
            ViewTables viewTables = new ViewTables(resultsForTime);
            viewTables.Show();

        }

    }
}