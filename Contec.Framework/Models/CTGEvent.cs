namespace Contec.Framework.Models
{
    public class CTGEvent
    {
        public string EventAbbreviation { get; set; }

        public string Payload { get; set; }

        public string TransactionKey { get; set; }

        public int SiteId { get; set; }

        public string StationId { get; set; }

        public string OperatorId { get; set; }
    }
}