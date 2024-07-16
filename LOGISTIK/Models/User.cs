using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGISTIK.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string WindowsAccount { get; set; }
        public string Role { get; set; }
        public bool IsEmailChecked { get; set; }
    }
}