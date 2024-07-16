using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace LOGISTIK.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Po_Machine { get; set; }
        public string Equipment_Type { get; set; }
        public string QTY { get; set; }
        public string Satuan_QTY { get; set; }
        public string Equipment_Gross { get; set; }
        public string Contact_Person { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Post_Code { get; set; }
        public string Country { get; set; }
        public string Email_Address { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Pickup_Time { get; set; }
        public string Status { get; set; } 
        public string Invoice { get; set; }
        public byte[] InvoiceFileData { get; set; }
        [DisplayFormat(DataFormatString = "{0: dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public string Confirm_Date {get;set;}
        public string Mode { get; set; }
        public string Location { get; set; }
    }
}