using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace ADMT___WPF.Utilities
{
    class UI_Utilities
    {
        // Get the domain name using IPGlobalProperties
        public string DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

        // LDAP path for the domain
        public string LDAP = "LDAP://DC=Domain,DC=COM"; // Replace with actual domain information

        // Check if a password meets the validation criteria
        public Boolean PasswordValidation(string pword)
        {
            if (pword.Length >= 8 && pword.Any(char.IsUpper) && pword.Any(char.IsLower) && pword.Any(char.IsNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if an account (user or computer) exists in the domain
        public Boolean doesAccountExist(string accountname, string type)
        {
            bool resultsfound = false;

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, DomainName);

            if (type == "User")
            {
                UserPrincipal up = new UserPrincipal(ctx);
                up.SamAccountName = accountname;

                resultsfound = FindAccount(ctx, up);
            }
            else if (type == "Computer")
            {
                ComputerPrincipal cp = new ComputerPrincipal(ctx);
                cp.Name = accountname;

                resultsfound = FindAccount(ctx, cp);
            }

            return resultsfound;
        }

        // Helper method to find an account using PrincipalSearcher
        private bool FindAccount<T>(PrincipalContext ctx, T principal) where T : Principal
        {
            using (PrincipalSearcher srch = new PrincipalSearcher(principal))
            {
                srch.QueryFilter = principal;

                PrincipalSearchResult<Principal> results = srch.FindAll();

                if (results != null)
                {
                    int resultCount = results.Count();

                    if (resultCount > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Update full name using first and last name
        public string FullNameUpdater(string first, string last)
        {
            return first + " " + last;
        }
    }
}
