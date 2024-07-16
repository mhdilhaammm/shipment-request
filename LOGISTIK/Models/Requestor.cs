using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGISTIK.Models
{
    public class Requestor
    {
        public int Id { get; set; }
        public string BadgeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Superior { get; set; }
        public string DateRequest { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}