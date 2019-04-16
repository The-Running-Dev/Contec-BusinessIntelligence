namespace Contec.Data.Models.Actions
{
    public class EmptyAction : GenericAction
    {
        public EmptyAction(string userId) : base(userId, AuditAction.None)
        {
        }
    }
}