using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Xrs
{
    static class Xs
    {
        public static bool flag = false;// pociag 
        public static Dictionary<int,AutoResetEvent> EventDown = new Dictionary<int, AutoResetEvent>();
        public static Dictionary<int, AutoResetEvent> EventUP = new Dictionary<int, AutoResetEvent>();
        public static int lenDown = 0;
        public static int lenUp = 0;
        public static Mutex mtx = new Mutex();
        public static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static void Paint(MainWindow main, List<Car> cars) {
            while (!CancellationTokenSource.Token.IsCancellationRequested) {
                main.Dispatcher.Invoke(() => {
                    main.canvas.Children.Clear();
                    List<Rectangle> rects = new List<Rectangle>();
                    List<Rectangle> train = new List<Rectangle>();
                    lock (mtx)
                    {
                        foreach (var auto in cars)
                        {
                            ImageBrush img = new ImageBrush();
                            BitmapImage bImg;
                            if (auto._downFlag)
                            {
                                bImg = new BitmapImage(new Uri("C:\\Users\\bohdan\\source\\repos\\Xrs\\Xrs\\pixil-frame-5.png"));
                            }
                            else {
                                bImg = new BitmapImage(new Uri("C:\\Users\\bohdan\\source\\repos\\Xrs\\Xrs\\pixil-frame-4.png"));
                            }
                            
                            img.ImageSource = bImg;
                            var square = new Rectangle
                            {
                                Width = auto._lenX,
                                Height = auto._lenY,
                                Fill = img//Brushes.Blue // You can set any color you want
                            };
                            Canvas.SetLeft(square, auto._x);

                            if (auto._downFlag)
                            {
                                Canvas.SetTop(square, auto._y);
                            }
                            else
                            {
                                Canvas.SetTop(square, auto._y + auto._lenY);
                            }

                            rects.Add(square);
                        }

                        if (Road.tr != null) {
                            
                            for (int i = 0; i < Road.tr._count; i++)
                            {
                                ImageBrush img = new ImageBrush();
                                BitmapImage bImg = new BitmapImage(new Uri("C:\\Users\\bohdan\\source\\repos\\Xrs\\Xrs\\pixil-frame-6.png"));
                                img.ImageSource = bImg;
                                Rectangle wag = new Rectangle
                                {
                                    Width = Road.tr._width,
                                    Height = Road.tr._height,
                                    Fill = img
                                };

                                int x = Road.tr._x - (i+2) * Road.tr._width;
                                Canvas.SetLeft(wag, x);
                                Canvas.SetTop(wag, Road.tr._y);
                                train.Add(wag);
                            }
                            if (Road.tr._x > 900 + Road.tr._width * Road.tr._count)
                            {
                                Road.tr = null;
                            }
                        }
                        
                    }

                    Rectangle Switch = new Rectangle
                    {
                        Width = 100,
                        Height = 100,
                    };
                    if (flag)
                    {
                        Switch.Fill = Brushes.Red;
                    }
                    else {
                        Switch.Fill = Brushes.Green;
                    }
                    Canvas.SetLeft (Switch, 20);
                    Canvas.SetTop (Switch, 700);
                    main.canvas.Children.Add(Switch);

                    foreach (var sq in rects) {
                        main.canvas.Children.Add(sq);
                    }
                    foreach (var wag in train) {
                        main.canvas.Children.Add(wag);
                    }
                    
                });
                
            }
        }

    }
}
