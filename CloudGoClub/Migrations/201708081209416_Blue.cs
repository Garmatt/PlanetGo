namespace CloudGoClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Blue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardSize",
                c => new
                    {
                        Id = c.Short(nullable: false, identity: true),
                        X = c.Short(nullable: false),
                        Y = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GameRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        IsOpen = c.Boolean(nullable: false),
                        CreationTimestamp = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        BoardSizeId = c.Short(nullable: false),
                        RuleSetId = c.Short(nullable: false),
                        ColorOfAuthor = c.Int(),
                        CompensationPoints = c.Double(),
                        PlayerWithCompensationPoints = c.Int(nullable: false),
                        HandicapStones = c.Short(),
                        HandicapStonesPlacement = c.Int(nullable: false),
                        LowestOpponentRankId = c.Short(),
                        HighestOpponentRankId = c.Short(),
                        SpecificOpponentId = c.Int(),
                        NewOpponentsOnly = c.Boolean(nullable: false),
                        TournamentId = c.Int(),
                        GameId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AuthorId)
                .ForeignKey("dbo.BoardSize", t => t.BoardSizeId)
                .ForeignKey("dbo.Game", t => t.GameId)
                .ForeignKey("dbo.Rank", t => t.HighestOpponentRankId)
                .ForeignKey("dbo.Rank", t => t.LowestOpponentRankId)
                .ForeignKey("dbo.RuleSet", t => t.RuleSetId)
                .ForeignKey("dbo.AspNetUsers", t => t.SpecificOpponentId)
                .ForeignKey("dbo.Tournament", t => t.TournamentId)
                .Index(t => t.AuthorId)
                .Index(t => t.BoardSizeId)
                .Index(t => t.RuleSetId)
                .Index(t => t.LowestOpponentRankId)
                .Index(t => t.HighestOpponentRankId)
                .Index(t => t.SpecificOpponentId)
                .Index(t => t.TournamentId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Rank_Id = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rank", t => t.Rank_Id, cascadeDelete: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Rank_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Rank",
                c => new
                    {
                        Id = c.Short(nullable: false),
                        Type = c.Int(nullable: false),
                        Level = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        GameRequestId = c.Long(nullable: false),
                        SgfString = c.String(),
                        BlackPlayerId = c.Int(nullable: false),
                        WhitePlayerId = c.Int(nullable: false),
                        BlackPlayerRankId = c.Short(nullable: false),
                        WhitePlayerRankId = c.Short(nullable: false),
                        StartTimestamp = c.DateTime(nullable: false),
                        LastPlayedTimestamp = c.DateTime(),
                        EndTimestamp = c.DateTime(),
                        Moves = c.Short(nullable: false),
                        Turn = c.Int(),
                        GameResultId = c.Long(),
                        Tournament_Id = c.Int(),
                    })
                .PrimaryKey(t => t.GameRequestId)
                .ForeignKey("dbo.AspNetUsers", t => t.BlackPlayerId)
                .ForeignKey("dbo.Rank", t => t.BlackPlayerRankId)
                .ForeignKey("dbo.GameRequest", t => t.GameRequestId)
                .ForeignKey("dbo.GameResult", t => t.GameResultId)
                .ForeignKey("dbo.AspNetUsers", t => t.WhitePlayerId)
                .ForeignKey("dbo.Rank", t => t.WhitePlayerRankId)
                .ForeignKey("dbo.Tournament", t => t.Tournament_Id)
                .Index(t => t.GameRequestId)
                .Index(t => t.BlackPlayerId)
                .Index(t => t.WhitePlayerId)
                .Index(t => t.BlackPlayerRankId)
                .Index(t => t.WhitePlayerRankId)
                .Index(t => t.GameResultId)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.GameResult",
                c => new
                    {
                        GameId = c.Long(nullable: false),
                        Type = c.Int(nullable: false),
                        WinnerScore = c.Short(),
                        LoserScore = c.Short(),
                        Winner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.GameId)
                .ForeignKey("dbo.Game", t => t.GameId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Winner_Id)
                .Index(t => t.GameId)
                .Index(t => t.Winner_Id);
            
            CreateTable(
                "dbo.RuleSet",
                c => new
                    {
                        Id = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tournament",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GameRequest", "TournamentId", "dbo.Tournament");
            DropForeignKey("dbo.Game", "Tournament_Id", "dbo.Tournament");
            DropForeignKey("dbo.GameRequest", "SpecificOpponentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameRequest", "RuleSetId", "dbo.RuleSet");
            DropForeignKey("dbo.GameRequest", "LowestOpponentRankId", "dbo.Rank");
            DropForeignKey("dbo.GameRequest", "HighestOpponentRankId", "dbo.Rank");
            DropForeignKey("dbo.GameRequest", "GameId", "dbo.Game");
            DropForeignKey("dbo.Game", "WhitePlayerRankId", "dbo.Rank");
            DropForeignKey("dbo.Game", "WhitePlayerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game", "GameResultId", "dbo.GameResult");
            DropForeignKey("dbo.GameResult", "Winner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameResult", "GameId", "dbo.Game");
            DropForeignKey("dbo.Game", "GameRequestId", "dbo.GameRequest");
            DropForeignKey("dbo.Game", "BlackPlayerRankId", "dbo.Rank");
            DropForeignKey("dbo.Game", "BlackPlayerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameRequest", "BoardSizeId", "dbo.BoardSize");
            DropForeignKey("dbo.GameRequest", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Rank_Id", "dbo.Rank");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.GameResult", new[] { "Winner_Id" });
            DropIndex("dbo.GameResult", new[] { "GameId" });
            DropIndex("dbo.Game", new[] { "Tournament_Id" });
            DropIndex("dbo.Game", new[] { "GameResultId" });
            DropIndex("dbo.Game", new[] { "WhitePlayerRankId" });
            DropIndex("dbo.Game", new[] { "BlackPlayerRankId" });
            DropIndex("dbo.Game", new[] { "WhitePlayerId" });
            DropIndex("dbo.Game", new[] { "BlackPlayerId" });
            DropIndex("dbo.Game", new[] { "GameRequestId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Rank_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.GameRequest", new[] { "GameId" });
            DropIndex("dbo.GameRequest", new[] { "TournamentId" });
            DropIndex("dbo.GameRequest", new[] { "SpecificOpponentId" });
            DropIndex("dbo.GameRequest", new[] { "HighestOpponentRankId" });
            DropIndex("dbo.GameRequest", new[] { "LowestOpponentRankId" });
            DropIndex("dbo.GameRequest", new[] { "RuleSetId" });
            DropIndex("dbo.GameRequest", new[] { "BoardSizeId" });
            DropIndex("dbo.GameRequest", new[] { "AuthorId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tournament");
            DropTable("dbo.RuleSet");
            DropTable("dbo.GameResult");
            DropTable("dbo.Game");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Rank");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.GameRequest");
            DropTable("dbo.BoardSize");
        }
    }
}
