using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace SharpImgur.Helpers
{
    public static class PasswordVaultExtensions
    {
        public static bool Contains(this PasswordVault vault, string resource, string userName)
        {
            try
            {
                vault.Retrieve(resource, userName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
