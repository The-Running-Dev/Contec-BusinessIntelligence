namespace Contec.Framework.Data
{
    public interface IDatabaseServer
    {
        bool IsConnectionValid(string connectionString);
    }
}