using System;

namespace Contec.Data.Models.Audits
{
    public class NotFoundAudit : Audit
    {
        public NotFoundAudit(string userId)
        {
            ActionId = AuditAction.Error;
            Action = Enum.GetName(typeof(AuditAction), AuditAction.Error);
            CreatedBy = userId;
        }
    }
}