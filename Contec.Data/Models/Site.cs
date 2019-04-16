namespace Contec.Data.Models
{
    public class Site
    {
        public int SiteId { get; set; }

        public string SiteName { get; set; }

        public int ParentId { get; set; }
    }
}