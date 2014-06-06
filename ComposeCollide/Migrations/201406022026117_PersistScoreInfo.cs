namespace ComposeCollide.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersistScoreInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScoreDetails", "ScoreInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScoreDetails", "ScoreInfo");
        }
    }
}
