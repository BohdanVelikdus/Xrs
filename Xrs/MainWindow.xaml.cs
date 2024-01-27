using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Xrs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Car> cars = new List<Car>();
        public MainWindow()
        {
            InitializeComponent();
            List<Thread> thr = new List<Thread>();
            Closing += MainWindow_Closing;

            Thread drawing = new Thread(() => Xs.Paint(this, cars));
            drawing.Start();
            Thread addingCar = new Thread(() => Road.generateCar(ref cars, ref thr));
            addingCar.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Xs.flag = !Xs.flag;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Xs.CancellationTokenSource.Cancel();
        }

    }
}