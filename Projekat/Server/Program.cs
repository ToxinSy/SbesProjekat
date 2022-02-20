using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Policy;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = SecretKey.GenerateKey();
            SecretKey.StoreKey(key, DES.KeyLocation);

            NetTcpBinding bindingClient = new NetTcpBinding();
            string addressClient = "net.tcp://localhost:5000/Options";

            bindingClient.Security.Mode = SecurityMode.Message; //Safer but slower then SecurityMode.Transport as it encrypts each message separately
            bindingClient.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows; //Based on windows user accounts
            bindingClient.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign; //Anti-Tampering signature (per message) protection

            ServiceHost host = new ServiceHost(typeof(Options));
            host.AddServiceEndpoint(typeof(IManagement), bindingClient, addressClient);

            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            host.Open();
            Console.WriteLine($"Options servis successfully started by [{WindowsIdentity.GetCurrent().User}] -> " + WindowsIdentity.GetCurrent().Name + ".\n");
            Console.ReadLine();

            host.Close();
        }
    }
}
