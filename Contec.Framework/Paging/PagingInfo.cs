namespace Contec.Framework.Paging
{
    public class PagingInfo
    {
        public int Number { get; set; }

        public int Size { get; set; }

        public string OrderBy { get; set; }

        public int Low()
        {
            var low = (Number - 1) * Size;

            return (low == 0) ? 1 : low + 1;
        }

        public int High()
        {
            return Low() + Size - 1;
        }
    }
}