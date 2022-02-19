using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static bool isTimerRunning;
        private static Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {

            NetTcpBinding binding = new NetTcpBinding();
            string serverAddress = "net.tcp://localhost:5000/Options";
            
            binding.Security.Mode = SecurityMode.Message; //Safer but slower then SecurityMode.Transport as it encrypts each message separately
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; //Based on windows user accounts
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; //Anti-Tampering signature (per message) protection

            Console.WriteLine($"Currently used by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + "\n");

            using (ServerProxy serverProxy = new ServerProxy(binding, serverAddress))
            {

                Menu();

                CancellationTokenSource cts = new CancellationTokenSource();
                var task = new Task(() => ShowTimerInLine(cts));
                task.Start();

                while (!task.IsCompleted)
                {
                    var keyInput = Console.ReadKey(true);
                    if (!Console.KeyAvailable)
                    {
                        if (keyInput.Key == ConsoleKey.D1)
                        {
                            if (!isTimerRunning) 
                            { 
                                sw.Start();
                                Console.ForegroundColor = ConsoleColor.Green;
                                ShowMessage("1 pressed: Timer is running.");
                                isTimerRunning = !isTimerRunning;
                            }
                        }
                        else if (keyInput.Key == ConsoleKey.D2)
                        {
                            if (isTimerRunning)
                            {
                                sw.Stop();
                                Console.ForegroundColor = ConsoleColor.Red;
                                ShowMessage("2 pressed: Timer is stoped.");
                                isTimerRunning = !isTimerRunning;
                            }  
                        }
                        else if (keyInput.Key == ConsoleKey.D3)
                        {
                            isTimerRunning = false;
                            sw.Reset();
                            Console.ForegroundColor = ConsoleColor.Green;
                            ShowMessage("3 pressed: Reset timer in 1s.");
                            Task.Delay(1000).Wait();
                            Menu();
                        }
                        else if (keyInput.Key == ConsoleKey.D4)
                        {
                            isTimerRunning = true;
                            Console.ForegroundColor = ConsoleColor.Green;
                            ShowMessage("4 pressed: Set timer.");
                            Task.Delay(1000).Wait();
                            
                        }
                        else if (keyInput.Key == ConsoleKey.D5)
                        {
                            isTimerRunning = false;
                            sw.Reset();
                            Console.ForegroundColor = ConsoleColor.Green;
                            ShowMessage("5 pressed: Reset timer in 1s.");
                            Task.Delay(1000).Wait();
                            Menu();
                        }
                        else if (keyInput.Key == ConsoleKey.D6)
                        {
                            cts.Cancel();
                            Menu();
                            ShowMessage("6 pressed: Exiting program in 1s.");
                            Task.Delay(1000).Wait();
                            break;
                        }

                        Task.Delay(35).Wait();
                    }
                }
            }
        }

        private static void Menu()
        {
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine("-----OPTIONS-----\n");
            Console.WriteLine("1. Start Timer");
            Console.WriteLine("2. Stop Timer");
            Console.WriteLine("3. Reset Timer");
            Console.WriteLine("4. Set Timer");
            Console.WriteLine("5. Load Timer");
            Console.WriteLine("6. Exit");
        }
        private static void ShowMessage(string msg)
        {
            Console.SetCursorPosition(0, 16);
            Console.WriteLine("{0,-100}", msg);
        }
        static void ShowTimerInLine(CancellationTokenSource _cts)
        {
            int min = 60;
            int sec = 60;
            int milisec = 1000;

            Task.Delay(1).Wait();
            while (!_cts.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, 18);
                if (isTimerRunning)
                {
                    if (sw.ElapsedMilliseconds != 0)
                    {
                        var minute = (sw.ElapsedMilliseconds / (sec * milisec)) % min;
                        var seconds = (sw.ElapsedMilliseconds / milisec) % sec;
                        var miliSec = sw.ElapsedMilliseconds % milisec;

                        Console.WriteLine("{0,2:0#}:{1,2:0#}:{2,-100:0##}", minute, seconds, miliSec);
                    }
                }
                Task.Delay(1).Wait();
            }
        }
    }
}
