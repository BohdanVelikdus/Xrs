namespace Xrs
{
    internal class Train
    {
        public int _speed;
        public int _count;
        public int _x;
        public int _y;
        public int _width;
        public int _height = 50;
        public int _len;

        public Train(int x, int y, int speed, int count, int width) {
            _x = x;
            _y = y;
            _speed = speed;
            _count = count;
            _width = width;
        }

        public void move() {
            while (!Xs.CancellationTokenSource.IsCancellationRequested) {
                if (_x >= -_speed * _count - _speed * _width) {
                    Xs.flag = true;
                }
                if (_x >= 500 + _width * _count) {
                    Xs.flag = false;
                }
                if (_x > 900 + _width * _count) {
                    break;
                }
                _x+= _speed;
                Thread.Sleep(30);
            }
        } 
    }
}
