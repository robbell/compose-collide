namespace ComposeCollide.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePlayedNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScoreDetails", "Played", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ScoreDetails", "Played", c => c.DateTime(nullable: false));
        }
    }
}
