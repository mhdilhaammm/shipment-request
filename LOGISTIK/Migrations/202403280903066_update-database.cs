namespace LOGISTIK.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("LOGISTIC_ShipmentRequest.Cargoes", "Id_Equipment", c => c.Int(nullable: false));
            AddColumn("LOGISTIC_ShipmentRequest.Dimensions", "Id_Equipment", c => c.Int(nullable: false));
            AddColumn("LOGISTIC_ShipmentRequest.Equipments", "Id_Requestor", c => c.Int(nullable: false));
            AddColumn("LOGISTIC_ShipmentRequest.Equipments", "Id_EmailsForwarder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("LOGISTIC_ShipmentRequest.Equipments", "Id_EmailsForwarder");
            DropColumn("LOGISTIC_ShipmentRequest.Equipments", "Id_Requestor");
            DropColumn("LOGISTIC_ShipmentRequest.Dimensions", "Id_Equipment");
            DropColumn("LOGISTIC_ShipmentRequest.Cargoes", "Id_Equipment");
        }
    }
}
