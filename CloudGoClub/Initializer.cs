using CloudGoClub.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace CloudGoClub
{
    public class Initializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var ranks = new List<Rank>(34);
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
            ranks.ForEach(r => context.Ranks.Add(r));
            context.SaveChanges();

            var adminUser = new ApplicationUser { UserName = "PlanetGoAdmin", Email = "mattia.garassini@gmail.com", Rank = context.Ranks.Find(0) };
            context.Users.Add(adminUser);
            context.SaveChanges();

            var sizes = new List<BoardSize> { 
                new BoardSize{ X = 9, Y = 9 },
                new BoardSize{ X = 13, Y = 13 },
                new BoardSize{ X = 19, Y = 19 }
            };
            sizes.ForEach(s => context.BoardSizes.Add(s));
            context.SaveChanges();
        }
    }
}