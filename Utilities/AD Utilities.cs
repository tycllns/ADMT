using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;

class AD_Utilities
{
    private string _domain;
    private string username;
    private string password;

    public AD_Utilities(string domain)
    {
        _domain = domain;
    }

    // Create a new user in Active Directory
    public void CreateUser(string username, string givenname, string lastname, string description, string employeeid, string fullname, string emailaddress, bool cantchangepassword, bool passwordneverexpires, bool changepasswordatlogon, bool enabled)
    {
        using (PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain, username, password))
        {
            UserPrincipal user = new UserPrincipal(context);
            user.SamAccountName = username;
            user.GivenName = givenname;
            user.Surname = lastname;
            user.Description = description;
            user.EmployeeId = employeeid;
            user.DisplayName = fullname;
            user.EmailAddress = emailaddress;
            user.UserCannotChangePassword = cantchangepassword;
            user.PasswordNeverExpires = passwordneverexpires;
            user.Enabled = enabled;

            // Set additional properties if needed

            user.Save();
        }
    }

    // Delete a user from Active Directory
    public void DeleteUser(string username)
    {
        using (PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain, username, password))
        {
            UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
            if (user != null)
            {
                user.Delete();
            }
        }
    }

    // Add a user to a group
    public void AddUserToGroup(string username, string groupName)
    {
        using (PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain, username, password))
        {
            UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
            if (user != null)
            {
                GroupPrincipal group = GroupPrincipal.FindByIdentity(context, groupName);
                if (group != null)
                {
                    group.Members.Add(user);
                    group.Save();
                }
            }
        }
    }

    // Change user password
    public void ChangeUserPassword(string username, string newPassword)
    {
        using (PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain, username, password))
        {
            UserPrincipal user = UserPrincipal.FindByIdentity(context, username);
            if (user != null)
            {
                user.SetPassword(newPassword);
                user.Save();
            }
        }
    }
}
