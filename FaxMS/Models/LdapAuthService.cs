using System;
using System.DirectoryServices.Protocols;
using System.Net;

namespace FaxMS.Models
{
    /// <summary>
    /// LDAP 驗證服務
    /// </summary>
    public class LdapAuthService
    {
        private readonly string _ldapServer;
        private readonly int _ldapPort;
        private readonly string _domain;

        public LdapAuthService(string ldapServer, int ldapPort, string domain)
        {
            _ldapServer = ldapServer;
            _ldapPort = ldapPort;
            _domain = domain;
        }

        public bool ValidateUser(string account, string password)
        {
            try
            {
                using (var ldap = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer, _ldapPort)))
                {
                    ldap.AuthType = AuthType.Negotiate;
                    var credential = new NetworkCredential($"{_domain}\\{account}", password);
                    ldap.Bind(credential);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}