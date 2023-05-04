namespace Template.Backend.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RowVersion = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        Name = c.String(nullable: false, maxLength: 256),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true, name: "UI_Name");
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RowVersion = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        Name = c.String(nullable: false, maxLength: 256),
                        Address = c.String(nullable: false),
                        Phone = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        CompanyID = c.Int(nullable: false),
                        DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .ForeignKey("dbo.Companies", t => t.CompanyID)
                .Index(t => t.Name, unique: true, name: "UI_Name")
                .Index(t => t.CompanyID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RowVersion = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name, unique: true, name: "UI_Name");
            
            CreateTable(
                "dbo.CompanyAudit",
                c => new
                    {
                        CompanyAuditID = c.Int(nullable: false, identity: true),
                        RowVersion = c.Int(nullable: false),
                        ID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        AuditOperation = c.Int(nullable: false),
                        LoggedUserName = c.String(),
                        Name = c.String(),
                        CreationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyAuditID);
            
            CreateTable(
                "dbo.DepartmentAudit",
                c => new
                    {
                        DepartmentAuditID = c.Int(nullable: false, identity: true),
                        RowVersion = c.Int(nullable: false),
                        ID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        AuditOperation = c.Int(nullable: false),
                        LoggedUserName = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentAuditID);
            
            CreateTable(
                "dbo.EmployeeAudit",
                c => new
                    {
                        EmployeeAuditID = c.Int(nullable: false, identity: true),
                        RowVersion = c.Int(nullable: false),
                        ID = c.Int(nullable: false),
                        CreatedDate = c.DateTime(),
                        CreatedOn = c.DateTime(),
                        AuditOperation = c.Int(nullable: false),
                        LoggedUserName = c.String(),
                        Name = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        BirthDate = c.DateTime(),
                        CompanyID = c.Int(),
                        DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeAuditID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Employees", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Departments", "UI_Name");
            DropIndex("dbo.Employees", new[] { "DepartmentID" });
            DropIndex("dbo.Employees", new[] { "CompanyID" });
            DropIndex("dbo.Employees", "UI_Name");
            DropIndex("dbo.Companies", "UI_Name");
            DropTable("dbo.EmployeeAudit");
            DropTable("dbo.DepartmentAudit");
            DropTable("dbo.CompanyAudit");
            DropTable("dbo.Departments");
            DropTable("dbo.Employees");
            DropTable("dbo.Companies");
        }
    }
}
