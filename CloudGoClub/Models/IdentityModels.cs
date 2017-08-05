using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CloudGoClub.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        public virtual Rank Rank { get; set; }

        //public virtual ICollection<Game> Games { get; set; }
    }

    public class ApplicationUserClaim : IdentityUserClaim<int> { }
    public class ApplicationUserRole : IdentityUserRole<int> { }
    public class ApplicationUserLogin : IdentityUserLogin<int> { }

    public class ApplicationRole : IdentityRole<int, ApplicationUserRole> { }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region GameRequest

            modelBuilder.Entity<GameRequest>()
                .HasRequired<ApplicationUser>(gameRequest => gameRequest.Author)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GameRequest>()
                .HasRequired<BoardSize>(gameRequest => gameRequest.BoardSize)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GameRequest>()
                .HasRequired<RuleSet>(gameRequest => gameRequest.RuleSet)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GameRequest>()
                .HasOptional<Rank>(gameRequest => gameRequest.LowestOpponentRank);

            modelBuilder.Entity<GameRequest>()
                .HasOptional<Rank>(gameRequest => gameRequest.HighestOpponentRank);

            modelBuilder.Entity<GameRequest>()
                .HasOptional<ApplicationUser>(gameRequest => gameRequest.SpecificOpponent);

            modelBuilder.Entity<GameRequest>()
                .HasOptional<Game>(gameRequest => gameRequest.Game);

            modelBuilder.Entity<GameRequest>()
                .HasOptional<Tournament>(gameRequest => gameRequest.Tournament);

            #endregion GameRequest

            #region Game

            modelBuilder.Entity<Game>()
                .HasRequired<ApplicationUser>(game => game.BlackPlayer)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasRequired<ApplicationUser>(game => game.WhitePlayer)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasRequired<GameRequest>(game => game.GameRequest)
                .WithRequiredDependent()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasRequired<Rank>(game => game.BlackPlayerRank)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasRequired<Rank>(game => game.WhitePlayerRank)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasOptional<GameResult>(game => game.GameResult);

            #endregion Game

            modelBuilder.Entity<GameResult>()
                .HasRequired<Game>(gameResult => gameResult.Game)
                .WithRequiredDependent()
                .WillCascadeOnDelete(true);
        }

        public DbSet<RuleSet> RuleSets { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<BoardSize> BoardSizes { get; set; }
        public DbSet<GameRequest> GameRequests { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameResult> GameResults { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
    }
}