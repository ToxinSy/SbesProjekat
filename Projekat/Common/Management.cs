using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IManagement
    {
        [OperationContract]
        void StartTimer();

        [OperationContract]
        void StopTimer();

        [OperationContract]
        void ResetTimer();

        [OperationContract]
        void SetTimer(byte[] array);

        [OperationContract]
        void PrintTimer();

        [OperationContract]
        void TimerThread();
    }
}
