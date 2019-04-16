﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BI.Web.Models
{
    public class LogOnModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember?")]
        public bool RememberMe { get; set; }
    }
}