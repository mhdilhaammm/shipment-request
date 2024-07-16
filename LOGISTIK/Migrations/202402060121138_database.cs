namespace LOGISTIK.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LOGISTIC_ShipmentRequest.Cargoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Q1 = c.String(),
                        Q2 = c.String(),
                        Q3 = c.String(),
                        Q4 = c.String(),
                        Q5 = c.String(),
                        Q6 = c.String(),
                        Q7 = c.String(),
                        Q8 = c.String(),
                        Q9 = c.String(),
                        Q10 = c.String(),
                        Q11 = c.String(),
                        Q12 = c.String(),
                        Q13 = c.String(),
                        Q14 = c.String(),
                        Q15 = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "LOGISTIC_ShipmentRequest.Dimensions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Length = c.String(),
                        Weight = c.String(),
                        Height = c.String(),
                        Length_2 = c.String(),
                        Weight_2 = c.String(),
                        Height_2 = c.String(),
                        Length_3 = c.String(),
                        Weight_3 = c.String(),
                        Height_3 = c.String(),
                        Length_4 = c.String(),
                        Weight_4 = c.String(),
                        Height_4 = c.String(),
                        Length_5 = c.String(),
                        Weight_5 = c.String(),
                        Height_5 = c.String(),
                        Length_6 = c.String(),
                        Weight_6 = c.String(),
                        Height_6 = c.String(),
                        Length_7 = c.String(),
                        Weight_7 = c.String(),
                        Height_7 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "LOGISTIC_ShipmentRequest.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EmailForwarder = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "LOGISTIC_ShipmentRequest.Equipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Po_Machine = c.String(),
                        Equipment_Type = c.String(),
                        QTY = c.String(),
                        Satuan_QTY = c.String(),
                        Equipment_Gross = c.String(),
                        Contact_Person = c.String(),
                        Phone = c.String(),
                        Company = c.String(),
                        Address = c.String(),
                        Post_Code = c.String(),
                        Country = c.String(),
                        Email_Address = c.String(),
                        Pickup_Time = c.DateTime(nullable: false),
                        Status = c.String(),
                        Invoice = c.String(),
                        InvoiceFileData = c.Binary(),
                        Confirm_Date = c.String(),
                        Mode = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "LOGISTIC_ShipmentRequest.Requestors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BadgeId = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Department = c.String(),
                        Superior = c.String(),
                        DateRequest = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "LOGISTIC_ShipmentRequest.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Type = c.String(),
                        WindowsAccount = c.String(),
                        Role = c.String(),
                        IsEmailChecked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("LOGISTIC_ShipmentRequest.Users");
            DropTable("LOGISTIC_ShipmentRequest.Requestors");
            DropTable("LOGISTIC_ShipmentRequest.Equipments");
            DropTable("LOGISTIC_ShipmentRequest.Emails");
            DropTable("LOGISTIC_ShipmentRequest.Dimensions");
            DropTable("LOGISTIC_ShipmentRequest.Cargoes");
        }
    }
}
