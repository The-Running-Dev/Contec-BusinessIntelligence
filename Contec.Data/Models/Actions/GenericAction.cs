using System;
using Contec.Data.Models.Audits;

namespace Contec.Data.Models.Actions
{
    public interface IGenericAction
    {
        string UserId { get; set; }

        IAudit Audit { get; set; }
    }

    public class GenericAction : IGenericAction
    {
        public string UserId { get; set; }

        public IAudit Audit { get; set; }

        protected GenericAction(string userId, AuditAction action)
        {
            UserId = userId;
            Audit = new Audit()
            {
                ActionId = action,
                Action = Enum.GetName(typeof(AuditAction), action),
                CreatedBy = userId
            };
        }
    }
}