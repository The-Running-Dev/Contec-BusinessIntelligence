using Contec.Data.ViewModel;

namespace Contec.Data.Models.Actions
{
    public class CreateAction : BIAction<BIReportViewModel>
    {
        public CreateAction(BIReportViewModel model, string userId) : base(userId, AuditAction.Create)
        {
            Param = model;
        }
    }
}