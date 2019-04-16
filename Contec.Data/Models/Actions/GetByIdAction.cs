using System;

namespace Contec.Data.Models.Actions
{
    public class GetByIdAction : BIAction<Guid>
    {
        public GetByIdAction(Guid id, string userId) : base(userId, AuditAction.View)
        {
            Param = id;
        }
    }
}