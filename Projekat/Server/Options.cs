using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Options : IManagement
    {
        static int hours = -1;
        static int minutes = -1;
        static int seconds = -1;
        static int allSeconds = -1;
        static Thread timerThread = null;

        [PrincipalPermission(SecurityAction.Demand, Role = "StartStop")]
        public void StartTimer()
        {
            if (allSeconds > 0)
            {
                timerThread = new Thread(TimerThread);
                timerThread.Start();
                Console.WriteLine(" [ TIMER STARTED ] ");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "StartStop")]
        public void StopTimer()
        {
            timerThread.Suspend();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Change")]
        public void ResetTimer()
        {
            hours = 0;
            minutes = 0;
            seconds = 0;
            allSeconds = 0;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Change")]
        public void SetTimer(byte[] array)
        {
            string timer = DES.DecryptFile(array, SecretKey.LoadKey(DES.KeyLocation));
            string[] parts = timer.Split(':');
            hours = Int32.Parse(parts[0]);
            minutes = Int32.Parse(parts[1]);
            seconds = Int32.Parse(parts[2]);
            allSeconds = hours * 3600 + minutes * 60 + seconds;
            Console.WriteLine(" [ TIMER SET ] ");
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "See")]
        public void PrintTimer()
        {
            Console.WriteLine("all = " + allSeconds);
            int hh = allSeconds / 3600;
            int mm = (allSeconds - (hh * 3600)) / 60;
            int ss = allSeconds - (hh * 3600) - (mm * 60);

            Console.WriteLine($" [ TIMER ] => {hh.ToString("D2")}:{mm.ToString("D2")}:{ss.ToString("D2")} ");
        }
        public void TimerThread()
        {
            while (allSeconds > 0)
            {
                Thread.Sleep(1000);
                allSeconds--;
            }
            Console.WriteLine(" [ WAKE UP ITS TIME FOR SCHOOL ]");
            StopTimer();
        }

    }
}
