using System;
using ComposeCollide.Models;

namespace ComposeCollide.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ComposeCollideContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ComposeCollide.Models.ComposeCollideContext";
        }

        protected override void Seed(ComposeCollideContext context)
        {
            context.ScoreDetails.AddOrUpdate(new ScoreDetail
            {
                Creator = "Rob",
                Created = DateTime.Now,
                ScoreInfo = "01010101,00000000,11111111,00000000"
            });
        }
    }
}
