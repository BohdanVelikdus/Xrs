namespace Xrs
{
    class Road
    {
        public static int _queueDown = 0;
        public static int _queueUp = 0;
        public static Random rnd = new Random();
        public static Train tr = null;
        
        public static void generateCar(ref List<Car> cars, ref List<Thread> threads) {
            int count = 0;
            while (!Xs.CancellationTokenSource.Token.IsCancellationRequested) { // token 
                Thread.Sleep(rnd.Next(4500, 6000));
                lock (Xs.mtx)
                {
                    if (rnd.Next(0, 6) % 2 == 0)
                    {
                        Car cr1 = new Car(300, 0, false, 50, 50, count);
                        Thread newCar = new Thread(() => cr1.move());
                        Xs.EventUP[count] = new AutoResetEvent(false);
                        cars.Add(cr1);
                        newCar.Start();
                        threads.Add(newCar);
                    }
                    else
                    {
                        Car cr1 = new Car(400, 900, true, 50, 50, count);
                        Thread newCar = new Thread(() => cr1.move());
                        Xs.EventDown[count] = new AutoResetEvent(false);
                        cars.Add(cr1);
                        newCar.Start();
                        threads.Add(newCar);
                    }
                    count++;
                }

                if (rnd.Next(0, 10) % 4 == 0&& tr == null) { 
                    int ct = rnd.Next(0, 10);
                    int speed = rnd.Next(3, 7);
                    int cout = rnd.Next(3, 6);
                    int wdth = rnd.Next(30, 90);
                    tr = new Train(-speed * cout - speed *wdth,425,speed,cout , wdth);
                    Thread thr = new Thread(() => tr.move());
                    thr.Start();
                }

            }
        }
    }
}
