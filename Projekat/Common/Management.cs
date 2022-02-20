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
        void SetTimer(string timer);

        [OperationContract]
        void PrintTimer();

        [OperationContract]
        void TimerThread();
    }
}
