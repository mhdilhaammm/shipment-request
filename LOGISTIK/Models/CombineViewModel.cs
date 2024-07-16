using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Web;

namespace LOGISTIK.Models
{
    public class CombineViewModel
    {
        //untuk list registrasi
        public Requestor RequestorModel { get; set; }
        public Equipment EquipmentModel { get; set; }
        public Dimension DimensionModel { get; set; }
        public Cargo CargoModel { get; set; }
        public Email EmailModel { get; set; }
        public User UserModel { get; set; }

        //untuk list table
        public List<Requestor> ListRequstor { get; set; }
        public List<Equipment> ListEquipment { get; set; }
        public List<Cargo> ListCargo { get; set; }
        public List<Dimension> ListDimension { get; set; }
        public List<Email> ListEmail { get; set; }
        public List<User> ListUser { get; set; }
        public string[] SelectedEmails { get; set; }
        public CombineViewModel()
        {
            ListEmail = new List<Email>();
        }

        //List Button Delete
        public List<Int> SelectedIds { get; set; }

        //Upload File
        public HttpPostedFileBase File { get; set; }

        //public List<string> SelectedEmails { get; set; }
    }
}