using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPlayground.Core.Services
{
    // iPlayground.Core/Services/USBScannerService.cs
    public class USBScannerService : IDisposable
    {
        private readonly ManualResetEvent _scanComplete = new ManualResetEvent(false);
        private string _scannedData;
        private bool _isListening;
        private readonly object _lockObject = new object();

        public event EventHandler<string> DataScanned;

        public void StartListening()
        {
            _isListening = true;
            Task.Run(() => ListenForScannerInput());
        }

        public void StopListening()
        {
            _isListening = false;
        }

        private void ListenForScannerInput()
        {
            StringBuilder buffer = new StringBuilder();

            while (_isListening)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        string data;
                        lock (_lockObject)
                        {
                            data = buffer.ToString();
                            buffer.Clear();
                        }

                        if (!string.IsNullOrEmpty(data))
                        {
                            _scannedData = data;
                            DataScanned?.Invoke(this, data);
                            _scanComplete.Set();
                        }
                    }
                    else
                    {
                        lock (_lockObject)
                        {
                            buffer.Append(key.KeyChar);
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

        public async Task<string> WaitForScan(int timeoutMilliseconds = 30000)
        {
            _scanComplete.Reset();
            if (_scanComplete.WaitOne(timeoutMilliseconds))
            {
                return _scannedData;
            }
            throw new TimeoutException("Isteklo vrijeme za skeniranje.");
        }

        public void Dispose()
        {
            StopListening();
            _scanComplete?.Dispose();
        }
    }
}
