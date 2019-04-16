using System;

namespace Contec.Data.Models.Audits
{
    public class ErrorAudit : Audit
    {
        public ErrorAudit(string userId)
        {
            ActionId = AuditAction.Error;
            Action = Enum.GetName(typeof(AuditAction), AuditAction.Error);
            CreatedBy = userId;
        }
    }
}