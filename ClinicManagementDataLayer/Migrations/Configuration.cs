namespace ClinicManagementDataLayer.Migrations
{
    using ClinicManagementSystemModels.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ClinicManagementDataLayer.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ClinicManagementDataLayer.Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var users = new List<UserModel>()
            {
                new UserModel{UserName="Admin",Password="e7cf3ef4f17c3999a94f2c6f612e8a888e5b1026878e4e19398b23bd38ec221a",FullName="Admin",EmailId="Admin@gmail.com",PhoneNo="7026743212",Address="Bangalore",Gender = GenderType.Male,City = City.Bangalore}
            };
            var roles = new List<Roles>()
            {
                new Roles{RoleName= "Admin"},
                new Roles{RoleName= "Doctor"},
                new Roles{RoleName= "Nurse"},
                new Roles{RoleName= "Patient"},
            };
            context.LoginUsers.AddRange(users);
            context.Roles.AddRange(roles);
            base.Seed(context);
        }
    }
}
