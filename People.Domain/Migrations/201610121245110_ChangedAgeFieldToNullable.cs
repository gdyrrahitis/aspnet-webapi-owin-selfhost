namespace People.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ChangedAgeFieldToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "Age", c => c.Int(nullable: true));
        }

        public override void Down()
        {
            AlterColumn("dbo.People", "Age", c => c.Int(nullable: false));
        }
    }
}
