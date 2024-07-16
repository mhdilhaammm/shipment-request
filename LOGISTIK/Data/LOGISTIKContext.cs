using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LOGISTIK.Data
{
    public class LOGISTIKContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public LOGISTIKContext() : base("name=LOGISTIKContext")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("LOGISTIC_ShipmentRequest");
        }
        public System.Data.Entity.DbSet<LOGISTIK.Models.Requestor> Requestors { get; set; }
        public System.Data.Entity.DbSet<LOGISTIK.Models.Equipment> Equipments { get; set; }
        public System.Data.Entity.DbSet<LOGISTIK.Models.Dimension> Dimensions { get; set; }
        public System.Data.Entity.DbSet<LOGISTIK.Models.Cargo> Cargos { get; set; }
        public System.Data.Entity.DbSet<LOGISTIK.Models.User> Users { get; set; }
        public System.Data.Entity.DbSet<LOGISTIK.Models.Email> Emails { get; set; }

    }
}
