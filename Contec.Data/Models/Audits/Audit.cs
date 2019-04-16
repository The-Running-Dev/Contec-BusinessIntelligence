namespace Contec.Data.Models.Audits
{
    public interface IAudit
    {
        AuditAction ActionId { get; set; }

        string Action { get; set; }
    }

    public class Audit : DataModel, IAudit
    {
        public AuditAction ActionId { get; set; }

        public string Action { get; set; }
    }
}