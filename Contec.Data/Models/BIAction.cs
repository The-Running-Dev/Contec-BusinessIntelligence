using Contec.Data.Models.Actions;

namespace Contec.Data.Models
{
    public class BIAction<T> : GenericAction
    {
        public T Param { get; protected set; }

        public BIAction(string userId, AuditAction action) : base(userId, action) { }
    }
}