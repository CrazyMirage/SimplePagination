namespace TestHandler.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TestHandler.DAL;
    using System.Web.Helpers;

    internal sealed class Configuration : DbMigrationsConfiguration<TestHandler.DAL.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestHandler.DAL.DataContext context)
        {
            Role user = new Role() { RoleName = "user" };
            Role admin = new Role() { RoleName = "admin" };

            context.Roles.Add(user);
            context.Roles.Add(admin);

            context.Users.Add(new User()
            {
                Username = "user",
                Email = "user@mail.ru",
                Password = Crypto.HashPassword("qwerty"),
                CreateDate = DateTime.Now,
                Roles = new List<Role>() { user }
            });

            context.Users.Add(new User()
            {
                Username = "admin",
                Email = "admin@mail.ru",
                Password = Crypto.HashPassword("root"),
                CreateDate = DateTime.Now,
                Roles = new List<Role>() { admin }
            });

        }
    }
}
