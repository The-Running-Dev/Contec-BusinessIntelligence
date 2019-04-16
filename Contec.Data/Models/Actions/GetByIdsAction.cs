using System.Collections.Generic;

namespace Contec.Data.Models.Actions
{
    public class GetByIdsAction : BIAction<List<int>>
    {
        public GetByIdsAction(string userId) : base(userId, AuditAction.List)
        {
        }
    }
}