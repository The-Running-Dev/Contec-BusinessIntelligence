using System;
using System.Security.Principal;

namespace Contec.Data.Models
{
    public interface IUserPrincipal : IPrincipal
    {
    }

    public class UserPrincipal : IUserPrincipal
    {
        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}