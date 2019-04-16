using System;

namespace Contec.Data.Models.Actions
{
    public class DeleteAction : BIAction<Guid>
    {
        public DeleteAction(Guid id, string userId) : base(userId, AuditAction.Delete)
        {
            Param = id;
        }
    }
}