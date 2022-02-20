using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server;

namespace Client
{
    class Program
    {
        
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

                int select = 0;
                while (true) 
                {
                    Console.WriteLine("-----OPTIONS-----\n");
                    Console.WriteLine(" [ 1 ] SetTimer");
                    Console.WriteLine(" [ 2 ] StartTimer");
                    Console.WriteLine(" [ 3 ] StopTimer");
                    Console.WriteLine(" [ 4 ] ResetTime");
                    Console.WriteLine(" [ 5 ] PrintTime\n\n");


                    var keyInput = Console.ReadKey(true);
                    if (!Console.KeyAvailable)
                    {
                        if (keyInput.Key == ConsoleKey.D1)
                        {
                            Console.WriteLine(" Enter hours :");
                            int hh = Int32.Parse(Console.ReadLine());
                            Console.WriteLine(" Enter minutes :");
                            int mm = Int32.Parse(Console.ReadLine());
                            Console.WriteLine(" Enter seconds :");
                            int ss = Int32.Parse(Console.ReadLine());
                            serverProxy.SetTimer($"{hh}:{mm}:{ss}");
                        }
                        else if (keyInput.Key == ConsoleKey.D2)
                        {
                            serverProxy.StartTimer();
                        }
                        else if (keyInput.Key == ConsoleKey.D3)
                        {
                            serverProxy.StopTimer();
                        }
                        else if (keyInput.Key == ConsoleKey.D4)
                        {
                            serverProxy.ResetTimer();
                        }
                        else if (keyInput.Key == ConsoleKey.D5)
                        {
                            serverProxy.PrintTimer();
                        }
                    }
                }
                
            }
        }
    }
}
