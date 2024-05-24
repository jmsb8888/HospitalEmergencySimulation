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
        private DispatcherTimer timer = new DispatcherTimer();
        List<Tuple<double, double>> coordinateHighPriority = new List<Tuple<double, double>>();
        List<Tuple<double, double>> coordinateLowPriority = new List<Tuple<double, double>>();
        int countHiPriority = 0;
        int countLowPriority = 0;

        public MainWindow()
        {
            InitializeComponent();
            coordinateHighPriority.Add(new Tuple<double, double>(0, 300));
            coordinateLowPriority.Add(new Tuple<double, double>(0, 300));
            ServerOne.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\file (1).png"));
            ServerTwo.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\file (2).png"));
            ServerThree.Source = new BitmapImage(new Uri("C:\\Users\\Jmsb-\\OneDrive\\Escritorio\\TallerElectivaIIUsers\\Proyecto_simulacion\\HospitalEmergencySimulation\\HospitalEmergencySimulation\\file (3).png"));
        }
         public void CreatePatient(double seconds)
        {
            // Cambia esto al número de segundos que quieres esperar
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(seconds) };
            timer.Tick += Timer_Low_prority;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            double seconds = 1; // Cambia esto al número de segundos que quieres esperar
            CreatePatient(seconds);
        }
        private void InitService(object sender, RoutedEventArgs e)
        {
            attend("HighPriority", 148); // 590, 380, 148
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
            Canvas.SetLeft(image, 10);
            canvas.Children.Add(image);
            double initialTop = canvas.ActualHeight - image.Height; // Cambia '50' por la altura de tus imágenes
            Canvas.SetTop(image, initialTop); // Establecer la posición inicial de la image
           

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
                    To = 177,// lastPosition, // Cambia esto a la posición Y donde quieres que se quede la image
                    Duration = TimeSpan.FromSeconds(1)
                };
                coordinateHighPriority.Add(new Tuple<double, double>(0, coordinateHighPriority[coordinateHighPriority.Count - 1].Item2-30));
               
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
                            if(count > 1)
                            {
                                isFirst = true;
                            }
                        }
                    }
                    double xc = canvas.ActualWidth - count * 30;
                    myLabel.Content = "posicion " + xc + "prueba "+ isFirst;
                    positionFinal = isFirst ? 691 - count*30 : 691;

                    // Animación de movimiento horizontal
                    DoubleAnimation moverX = new DoubleAnimation
                    {

                        From = x,
                        To = positionFinal,
                        Duration = TimeSpan.FromSeconds(1)

                    };
                    image.BeginAnimation(Canvas.LeftProperty, moverX);
                };
                image.BeginAnimation(Canvas.TopProperty, moverY);

                
                //myLabel.Content = "Nuevo contenido  " + lastPosition + " CANTIDAD " + coordinateHighPriority.Count;
            };
            timer.Stop();
        }


        public void attend(string namePriority, double server)
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
                    Duration = TimeSpan.FromSeconds(1)
                };

                mover.Completed += (s, e) =>
                {
                    DoubleAnimation moverX = new DoubleAnimation
                    {

                        From = x,
                        To = server,//590,
                        Duration = TimeSpan.FromSeconds(1)

                    };
                    moverX.Completed += (s, e) =>
                    {
                        double yy = Canvas.GetTop(firstElement);
                        DoubleAnimation moverY = new DoubleAnimation
                        {

                            From = yy,
                            To = 103,
                            Duration = TimeSpan.FromSeconds(1)

                        };
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
            }
        }












        private void Timer_Low_prority(object sender, EventArgs e)
        {

            // Crear una nueva image y agregarla al canvas
            var image = new System.Windows.Controls.Image
            {
                Name = "LowPriority"+ (coordinateLowPriority.Count + 1),
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
                    To = 242,//lastPosition, // Cambia esto a la posición Y donde quieres que se quede la image
                    Duration = TimeSpan.FromSeconds(1)
                };
                coordinateLowPriority.Add(new Tuple<double, double>(0, coordinateLowPriority[coordinateLowPriority.Count - 1].Item2 - 30));
                image.BeginAnimation(Canvas.TopProperty, moverY);
                DoubleAnimation moverX = new DoubleAnimation
                {
                    From = initialTop,
                    To = 242,//lastPosition, // Cambia esto a la posición Y donde quieres que se quede la image
                    Duration = TimeSpan.FromSeconds(1)
                };
            };
            timer.Stop();
        }
    }
}