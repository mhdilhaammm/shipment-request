using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGISTIK.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailForwarder {get;set;}
        public string Type { get; set;}
    }
}