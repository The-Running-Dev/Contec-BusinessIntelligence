using System;
using System.Linq;
using System.Web.Profile;
using System.Web.Security;
using System.Collections.Generic;

namespace Contec.Data.Services
{
    public interface IAccountService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);

        MembershipCreateStatus CreateUser(string userName, string password, string email);

        bool ChangePassword(string userName, string oldPassword, string newPassword);

        List<int> GetAuthorizedSites(string userName);
    }

    public class AccountService : IAccountService
    {
        public AccountService(MembershipProvider provider)
        {
            _provider = provider;
        }

        public int MinPasswordLength => _provider.MinRequiredPasswordLength;

        public bool ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;

            _provider.CreateUser(userName, password, email, null, null, true, null, out status);

            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (string.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                var currentUser = _provider.GetUser(userName, true /* userIsOnline */);

                return currentUser != null && currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public List<int> GetAuthorizedSites(string userName)
        {
            //var user = _provider.GetUser(userName, true);
            var p = ProfileBase.Create(userName);
            var sites = (string) p.GetPropertyValue("AuthorizedSites");

            return sites.Split(',').Select(int.Parse).ToList();
        }

        private readonly MembershipProvider _provider;
    }
}