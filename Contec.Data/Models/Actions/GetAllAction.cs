namespace Contec.Data.Models.Actions
{
    public class GetAllAction : GenericAction
    {
        public GetAllAction(string userId) : base(userId, AuditAction.List) { }
    }
}