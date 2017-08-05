namespace CloudGoClub.Migrations
{
    using CloudGoClub.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CloudGoClub.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CloudGoClub.Models.ApplicationDbContext context)
        {
            context.RuleSets.AddOrUpdate(rs => rs.Id, new RuleSet { Id = 0 });
            context.SaveChanges();

            var ranks = new Rank[34];
            for (short i = 0; i < 34; i++)
            {
                if (i < 25)
                {
                    ranks[i] = new Rank { Id = i, Type = RankType.KYU, Level = (short)(25 - i) };
                }
                else
                {
                    ranks[i] = new Rank { Id = i, Type = RankType.DAN, Level = (short)(9 - (33 - i)) };
                }
            }
            context.Ranks.AddOrUpdate(r => r.Id, ranks);
            context.SaveChanges();

            var userManager = new ApplicationUserManager(new ApplicationUserStore(context));
            var noobRank = context.Ranks.Find(0);
            var adminUser = new ApplicationUser { UserName = "PlanetGoAdmin", Email = "mattia.garassini@gmail.com", //PasswordHash = new PasswordHasher().HashPassword("M4tt1a.15"), 
                Rank = noobRank, EmailConfirmed = true };
            var batmanUser = new ApplicationUser { UserName = "Batman", Email = "bruce.wayne@gothamcity.com", //PasswordHash = new PasswordHasher().HashPassword("nananana.nanananaX2"), 
                Rank = noobRank, EmailConfirmed = true };
            var elvisUser = new ApplicationUser { UserName = "Elvis", Email = "elvis.presley@memphis.com", //PasswordHash = new PasswordHasher().HashPassword("3lvisDaP3lvis"), 
                Rank = noobRank, EmailConfirmed = true };
            userManager.Create(adminUser, "M4tt1a.15");
            userManager.Create(batmanUser, "nananana.nanananaX2");
            userManager.Create(elvisUser, "3lvisDaP3lvis");
            context.Users.AddOrUpdate(u => u.UserName, adminUser, batmanUser, elvisUser);
            context.SaveChanges();

            var sizes = new BoardSize[3]; 
            sizes[0] = new BoardSize{ X = 9, Y = 9 };
            sizes[1] = new BoardSize{ X = 13, Y = 13 };
            sizes[2] = new BoardSize{ X = 19, Y = 19 };
            context.BoardSizes.AddOrUpdate(bs => bs.X, sizes);
            context.SaveChanges();
        }
    }
}
