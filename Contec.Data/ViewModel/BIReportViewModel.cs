using System;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contec.Data.ViewModel
{
    public class BIReportViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Exceeds 255 characters", MinimumLength = 5)]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Exceeds 1000 characters")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Embed Source")]
        [StringLength(1000, ErrorMessage = "Exceeds 1000 characters", MinimumLength = 5)]
        public string EmbedSource { get; set; }

        [Required]
        [DisplayName("Work Site")]
        public List<int> SiteIds { get; set; }

        public List<string> SiteNames { get; set; }

        public List<SelectListItem> AvailableSites { get; set; }

        [MaxLength(255, ErrorMessage = "Exceeds 255 characters")]
        public string CreatedBy { get; set; }
    }
}