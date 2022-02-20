using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Common
{
    public class CustomPrincipal : IPrincipal
    {
        WindowsIdentity identity = null;
        public CustomPrincipal(WindowsIdentity windowsIdentity)
        {
            identity = windowsIdentity;
        }
        public bool IsInRole(string permission)
        {
            foreach (IdentityReference group in this.identity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                string groupName = Formatter.ParseName(name.ToString());
                string[] permissions;
                if (RolesConfig.GetPermissions(groupName, out permissions))
                {
                    return permissions.Contains(permission);
                }
            }
            return false;
        }
        public IIdentity Identity
        {
            get { return identity; }
        }
    }
}
