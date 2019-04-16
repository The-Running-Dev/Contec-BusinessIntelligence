using System.Collections.Generic;

namespace Contec.Framework.Paging
{
    public class PagedDataResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int CurrentPage { get; set; }

        public int Total { get; set; }
    }
}