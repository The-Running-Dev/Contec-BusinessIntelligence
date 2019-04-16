namespace Contec.Data.Models
{
    public enum AuditAction
    {
        None = 0,
        List = 1,
        View = 2,
        Create = 3,
        Update = 4,
        Delete = 5,
        NotFound = 6,
        Error = 7
    }
}