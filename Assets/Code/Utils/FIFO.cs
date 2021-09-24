
namespace Code.Utils
{
    // Maybe we'll never use this 
    public class FIFO<T>
    {
        private T[] inFIFO;
        private int _currentSize;
        private int _maxSize;
        
        // FIFO constructor
        public FIFO(int size)
        {
            inFIFO = new T[size];
            _currentSize = 0;
            _maxSize = size;
        }
        
        public void Add(T obj)
        {
            if (!(_currentSize < _maxSize))
                MoveFIFO();
            
            inFIFO[_currentSize] = obj;
            _currentSize++;
        }
        
        public T GetLast() => inFIFO.Length > 0 ? inFIFO[0] : default(T);
        
        public T GetByIndex(int index) => index <= _currentSize ? inFIFO[index] : default(T);
        
        private void MoveFIFO()
        {
            for (int i = 1; i < _currentSize; i++)
                inFIFO[i - 1] = inFIFO[i];
        }
    }
}