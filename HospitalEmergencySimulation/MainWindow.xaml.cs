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
using System.IO;
using System;
namespace HospitalEmergencySimulation
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private DispatcherTimer timer;
        private DispatcherTimer timerHigh;
        List<Tuple<double, double>> coordinateHighPriority = new List<Tuple<double, double>>();
        List<Tuple<double, double>> coordinateLowPriority = new List<Tuple<double, double>>();
        ObservableCollection<ResultsForTime> resultsForTime = new ObservableCollection<ResultsForTime>();
        List<FormatPatient> formatPatientsHighPriority = new List<FormatPatient>();
        List<FormatPatient> formatPatientsLowPriorty = new List<FormatPatient>();
        List<FormatPatient> formatPatienttAll = new List<FormatPatient>();
        List<FormarDataDoctor> DataDoctor = new List<FormarDataDoctor>();
        Dictionary<int, FormatServers> servers = new Dictionary<int, FormatServers>();
        private int count = 0;
        private int countDelete = 0;
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
        Boolean ExistPatientForAttended = false;
        public MainWindow(ControllerSimulation controller)
        {
            this.controller = controller;
            InitializeComponent();
            coordinateHighPriority.Add(new Tuple<double, double>(0, 300));
            coordinateLowPriority.Add(new Tuple<double, double>(0, 300));
            Fondo.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../Hospital.png")));
            ServerOne.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../Sever1.png")));
            ServerTwo.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../Server2.png")));
            ServerThree.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../Server3.png")));
            ca.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoVetical.png")));
            ca2.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoVetical.png")));
            camino.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoVetical.png")));
            camino2.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoHorizontal.png")));
            camino22.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoHorizontal.png")));
            camino222.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoHorizontal.png")));
            camino3.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoHorizontal.png")));
            camino33.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoHorizontal.png")));
            camino333.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PisoHorizontal.png")));
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
                //myLabe.Content = Quantity.Count;
                numerbucle.Content = count;
                loadDataForTime(count);
                QuantityPatientientsHigh = patientsIdHigh.Count;
                QuantityPatientientsLow = patientsIdLow.Count;
                await ExecuteSimulationStep(QuantityPatientientsHigh, QuantityPatientientsLow);
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

        private async Task ExecuteSimulationStep(int high, int low)
        {
            // Crear pacientes y esperar 3.5 segundos
            cantidadPendiente.Content = "deben crear alta  " + high + "BAJA " + low;
            await CreatePatientAsync(2.5);
            await Task.Delay(TimeSpan.FromSeconds(3.5));
            // Iniciar el servicio y esperar tiempo adecuado
            await StartServiceAsync();
            await Task.Delay(TimeSpan.FromSeconds(1)); 
            // Terminar el servicio del paciente
            await TerminateServicePatienteAsync();
            await Task.Delay(TimeSpan.FromSeconds(3.5));
            if (countDelete > 0)
            {
                await atendNew(countDelete);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            countServerDesocupped();
            existPatients();
            if (countServersAvailable > 0 && ExistPatientForAttended)
            {
                await atendNew2(countServersAvailable);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
        private void existPatients()
        {
            var Format = formatPatienttAll.GroupBy(p => p.IdPatient)
                                    .Where(g => g.All(p => !p.IsAttended))
                                    .SelectMany(g => g)
                                    .ToList();
            foreach (FormatPatient patient in Format)
            {
                if (!patient.IsAttended)
                {
                    ExistPatientForAttended = true;
                    break;
                }
                ExistPatientForAttended = false;
            }
        }
        private async Task atendNew(int value)
        {
            int aux = 0;
            while (aux < value)
            {
                await StartServiceAsync();
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                aux++;
            }
            countDelete = 0;
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
            
            CreatePatient(interval); 
            await Task.Delay(TimeSpan.FromSeconds(interval)); 
        }

        private async Task StartServiceAsync()
        {
            
            // Ordenar la lista por IdPatient
            List<FormatPatient> orderedPatients = formatPatienttAll
                .OrderByDescending(p => p.Priority) 
                .ThenBy(p => p.TimeOfArrival) 
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
                           // myLabe.Content = $"Paciente movido, Id: {patient.IdPatient}, Prioridad: {patient.Priority}";
                            await Task.Delay(TimeSpan.FromSeconds(2));
                            break; 
                        }
                    }
                }
            }
        }


        private async Task TerminateServicePatienteAsync()
        {
            await TerminateServicePatiente(); 
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
                    foreach (KeyValuePair<int, FormatServers> item in servers)
                    {
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
                        To = server - 94, 
                        Duration = TimeSpan.FromSeconds(0.5)
                    };
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                    moverX.Completed += (s, e) =>
                    {
                        DoubleAnimation moverY = new DoubleAnimation
                        {
                            From = y,
                            To = -30, 
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
                    await tcs.Task; 
                }
            }
        }



        public void CreatePatient(double seconds)
        {


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

        /*
        public void CreatePatient(double seconds)
        {
            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(seconds);
                timer.Tick += Timer_Tick;
                timer.Tick += Timer_Low_prority;
            }
            timer.Start();
        }
        */
        private async void StartService()
        {
            List<FormatPatient> orderedPatients = formatPatienttAll
                .OrderByDescending(p => p.Priority) 
                .ThenBy(p => p.TimeOfArrival) 
                .ToList();
            int coundf = 0;
            foreach (FormatPatient patient in orderedPatients)
            {
               // myLabe.Content = "entro deberia mover " + 44;
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
                           // myLabe.Content = "entro deberia mover " + coundf;
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
                IsMovementLow = false; 
            }
            if (IsMovementHigh)
            {
                AdvanceQueue("HighPriority", 691);
                IsMovementHigh = false; 
            }
        }

        public async void AdvanceQueue(string namePriority, int position)
        {
            List<Task> animationTasks = new List<Task>();
            foreach (UIElement element in canvas.Children)
            {
                if (element is System.Windows.Controls.Image image && image.Name.StartsWith(namePriority))
                {
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
                        animationTasks.Add(tcs.Task);
                    }
                }
            }
            await Task.WhenAll(animationTasks);
        }
        private void InitService(object sender, RoutedEventArgs e)
        {
            StartService();
        }

        private void TerminateService(object sender, RoutedEventArgs e)
        {
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            var image = new System.Windows.Controls.Image
            {
                Name = "HighPriority" + (coordinateLowPriority.Count + 1),
                Width = 30,
                Height = 30,
                Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PatientHigh.png"))) 
            };

            canvas.Children.Add(image);
            double initialTop = canvas.ActualHeight - image.Height; 
            Canvas.SetTop(image, initialTop); 
            Canvas.SetLeft(image, 10);
            image.Loaded += (s, _) =>
            {
                DoubleAnimation aparecer = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                image.BeginAnimation(UIElement.OpacityProperty, aparecer);
                double x = Canvas.GetLeft(image);
                double y = Canvas.GetTop(image);
                double lastPosition = coordinateHighPriority[coordinateHighPriority.Count - 1].Item2;
                DoubleAnimation moverY = new DoubleAnimation
                {
                    From = y,
                    To = gdf,
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
                            //myLabel.Content = "Posicion X: " + xc + " prueba: " + isFirst + " yy: " + yc;
                        }
                    }
                    positionFinal = isFirst ? Canvas.GetLeft(lastImage) - 30 : 691;
                    if (positionFinal > 15)
                    {
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
            };
            patientHighCount++;
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
                double x = Canvas.GetLeft(firstElement);
                double y = Canvas.GetTop(firstElement);
                DoubleAnimation mover = new DoubleAnimation
                {
                    From = y,
                    To = 128, 
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                mover.Completed += (s, e) =>
                {
                    DoubleAnimation moverX = new DoubleAnimation
                    {

                        From = x,
                        To = server,
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
                        moverY.Completed += (s, r) =>
                        {
                            firstElement.Name = "A" + namePriority;
                        };
                        firstElement.BeginAnimation(Canvas.TopProperty, moverY);
                    };
                    firstElement.BeginAnimation(Canvas.LeftProperty, moverX);
                    double finalX = Canvas.GetLeft(firstElement);
                    double finalY = Canvas.GetTop(firstElement);
                    coordinateHighPriority.Add(new Tuple<double, double>(finalX, finalY));
                };

                firstElement.BeginAnimation(Canvas.TopProperty, mover);
                await Task.Delay(TimeSpan.FromSeconds(1.2));
            }
        }

        private void Timer_Low_prority(object sender, EventArgs e)
        {
            var image = new System.Windows.Controls.Image
            {
                Name = "LowPriority" + (coordinateLowPriority.Count + 1),
                Width = 30,
                Height = 30,
                Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(@"../../../PatientLow.png"))) 
            };
            canvas.Children.Add(image);
            double initialTop = canvas.ActualHeight - image.Height;
            Canvas.SetTop(image, initialTop); 

            double initialLeft = 100; 
            Canvas.SetLeft(image, initialLeft);
            image.Loaded += (s, _) =>
            {
                DoubleAnimation aparecer = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                image.BeginAnimation(UIElement.OpacityProperty, aparecer);
                double x = Canvas.GetLeft(image);
                double y = Canvas.GetTop(image);
                double lastPosition = coordinateLowPriority[coordinateLowPriority.Count - 1].Item2;
                DoubleAnimation moverY = new DoubleAnimation
                {
                    From = initialTop,
                    To = positionInitialLowPriority,
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
                        //myLabel.Content = "Posvicion X: " + positionFinal + " prueba: " + isFirst + " yy: ";
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
                                    //myLabel.Content = "Posicionc X: " + xc + " prueba: " + isFirst + " yy: " + yc;
                                }
                            }
                            positionFinal = isFirst ? Canvas.GetTop(lastImageLowPriority) + 30 : 177;
                            if (positionFinal <= 230)
                            {

                               // myLabel.Content = "Posicion Y: " + positionFinal + " prueba: " + isFirst + " yy: ";
                                DoubleAnimation moverYY = new DoubleAnimation
                                {
                                    To = positionFinal,
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
                        //myLabel.Content = "Possaicion Y: " + positionFinal + " prueba: " + isFirst + " yy: ";
                        positionInitialLowPriority += 30;
                    }
                };
                image.BeginAnimation(Canvas.TopProperty, moverY);
            };
            patientLowCount++;
            if (patientLowCount >= QuantityPatientientsLow)
            {
                timer.Stop();
            }
        }

        public void ViewTables(object sender, RoutedEventArgs e)
        {
           // myLabe.Content = "envio " + resultsForTime.Count;
            ViewTables viewTables = new ViewTables(resultsForTime);
            viewTables.Show();
        }

    }
}