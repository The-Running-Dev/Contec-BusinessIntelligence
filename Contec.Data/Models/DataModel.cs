using System;

namespace Contec.Data.Models
{
    public interface IDataModel
    {
        string CreatedBy { get; set; }

        DateTime CreatedOn { get; set; }

        Guid Id { get; set; }

        string UpdatedBy { get; set; }

        DateTime UpdatedOn { get; set; }
    }

    public class DataModel : IDataModel
    {
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        protected DataModel()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now.ToUniversalTime();
            UpdatedOn = DateTime.Now.ToUniversalTime();
        }
    }
}