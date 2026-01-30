namespace TourismPlatformMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileAndFeedbackDbSets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgencyProfiles",
                c => new
                    {
                        AgencyId = c.Int(nullable: false, identity: true),
                        AgencyName = c.String(),
                        ServicesOffered = c.String(),
                        Description = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.AgencyId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId);
            
            CreateTable(
                "dbo.TouristProfiles",
                c => new
                    {
                        TouristId = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        ContactNumber = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.TouristId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TouristProfiles");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.AgencyProfiles");
        }
    }
}
