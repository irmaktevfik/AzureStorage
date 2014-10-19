using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class HomeModel
    {
        [Required]
        public string DocumentName { get; set; }
    }
}