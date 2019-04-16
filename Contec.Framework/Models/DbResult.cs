namespace Contec.Framework.Models
{
    public class DbResult<T>
    {
        public string Message { get; set; }

        public bool IsError { get; set; }

        public T Result { get; set; }
    }
}