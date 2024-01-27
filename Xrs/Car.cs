namespace Xrs
{
    class Car
    {
        public bool _downFlag; // ride from down 
        public int _x;
        public int _y;
        public int _lenX;
        public int _lenY;
        public int _speed = 4;
        public int _border;
        public bool _callOnce = false;
        public int _num;
        public bool _wrokingFlag = true;
        public Car(int x, int y, bool flag, int lenX, int lenY, int num)
        {
            _x = x;
            _y = y;
            _downFlag = flag;
            _lenX = lenX;
            _lenY = lenY;
            _num = num;
        }
        void check_border() {
            while (_wrokingFlag && _downFlag && !Xs.CancellationTokenSource.IsCancellationRequested)
            { 
                Xs.EventDown[_num].WaitOne();
                if (!_callOnce)
                {
                    _border -= Xs.lenDown;
                }
                Thread.Sleep(Xs.lenDown/_speed);
            }
            while (_wrokingFlag && !_downFlag && !Xs.CancellationTokenSource.IsCancellationRequested)
            {
                Xs.EventUP[_num].WaitOne();
                if (!_callOnce)
                {
                    _border -= Xs.lenUp;
                }
                Thread.Sleep(Xs.lenDown / _speed);
            }
        }
        void manageBorder() {
            lock (Xs.mtx) { 
                if (_downFlag)
                {
                    Road._queueDown -= _lenY + 20;
                    Xs.lenDown = _lenY + 20;
                    foreach (int key in Xs.EventDown.Keys) {
                        Xs.EventDown[key].Set();
                    }
                    Thread.Sleep(10);
                }
                else
                {
                    Road._queueUp -= _lenY + 20;
                    Xs.lenUp = _lenY + 20;
                    foreach (int key in Xs.EventUP.Keys)
                    {
                        Xs.EventUP[key].Set();
                    }
                    Thread.Sleep(10);
                }
            }
        }
        public void move() {
            if (_downFlag)
            {
                _border = Road._queueDown;
                Road._queueDown += _lenY + 20;
            }
            else {
                _border = Road._queueUp;
                Road._queueUp += _lenY + 20;
            }
            Thread mB = new Thread(() => check_border());
            mB.Start();
            while (!Xs.CancellationTokenSource.IsCancellationRequested)
            { // token
                switch (_downFlag)
                {
                    case true:
                        if (_y <= 500 -3)
                        {
                            if (!_callOnce)
                            {
                                _callOnce = true;
                                Thread thr = new Thread( () => manageBorder());
                                thr.Start();
                                
                            }
                            _y -= _speed;
                        }
                        else if (Xs.flag)
                        {
                            if (_y > 500 + _border)
                            {
                                _y -= _speed;
                            }
                        }
                        else
                        {
                            _y -= _speed;
                        }
                    break;
                    case false:
                        if (_y >= 300 +3)
                        {
                            if (!_callOnce)
                            {
                                _callOnce = true;
                                Thread thr = new Thread(() => manageBorder());
                                thr.Start();
                            }
                            _y += _speed;
                        }
                        else if (Xs.flag)
                        {
                            if (_y < 300 - _border)
                            {
                                _y += _speed;
                            }
                        }
                        else
                        {
                            _y += _speed;
                        }
                    break;
                }
                if (_downFlag)
                {
                    if (_y < -_lenY)
                    {
                        break;
                    }
                }
                else {
                    if (_y > 900 + _lenY)
                    {
                        break;
                    }
                }
                Thread.Sleep(70);
            }
            _wrokingFlag = false;
        }
    }
}
