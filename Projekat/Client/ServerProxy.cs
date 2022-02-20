using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using Common;

namespace Client
{
    public class ServerProxy : ChannelFactory<IManagement>, IManagement, IDisposable
    {
        IManagement factory;
        public ServerProxy(NetTcpBinding binding, string address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }
        public void StartTimer()
        {
            factory.StartTimer();
        }

        public void StopTimer()
        {
            factory.StopTimer();
        }

        public void ResetTimer()
        {
            factory.ResetTimer();
        }

        public void SetTimer(byte[] array)
        {
            factory.SetTimer(array);
        }

        public void PrintTimer()
        {
            factory.PrintTimer();
        }

        public void TimerThread()
        {
            factory.TimerThread();
        }

    }
}
