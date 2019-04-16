using Contec.Data.ViewModel;

namespace Contec.Data.Models.Actions
{
    public class UpdateAction : BIAction<BIReportViewModel>
    {
        public UpdateAction(BIReportViewModel model, string userId) : base(userId, AuditAction.Update)
        {
            Param = model;
        }
    }
}