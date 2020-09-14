using System;
using System.Collections.Generic;
using System.Text;

namespace HealthApi
{
    public interface ISamsungHealthApiService
    {
        event EventHandler<SamsungEventArgs> OnSamsungHealthApiValueChanged;
        void Connect();
    }

    public class SamsungEventArgs : EventArgs
    {
        public int Count { get; set; }
    }
}
