namespace Data.Migrations
{
    using MySql.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

            // register mysql code generator
            SetSqlGenerator("MySql.Data.MySqlClient", new SqlGenerator());

            SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema)); //here s the thing.
        }

        protected override void Seed(Data.EFDbContext context)
        {
            //To Do
        }
    }
}
