namespace CloudGoClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
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
                "dbo.Game",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SgfString = c.String(),
                        BoardSizeId = c.Short(nullable: false),
                        BlackPlayerId = c.Int(nullable: false),
                        WhitePlayerId = c.Int(nullable: false),
                        BlackPlayerRankId = c.Short(nullable: false),
                        WhitePlayerRankId = c.Short(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        CreationTimestamp = c.DateTime(nullable: false),
                        StartTimestamp = c.DateTime(),
                        LastPlayedTimestamp = c.DateTime(),
                        EndTimestamp = c.DateTime(),
                        Moves = c.Short(nullable: false),
                        Turn = c.Int(),
                        GameResultId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.BlackPlayerId)
                .ForeignKey("dbo.Rank", t => t.BlackPlayerRankId)
                .ForeignKey("dbo.BoardSize", t => t.BoardSizeId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.GameResult", t => t.GameResultId)
                .ForeignKey("dbo.AspNetUsers", t => t.WhitePlayerId)
                .ForeignKey("dbo.Rank", t => t.WhitePlayerRankId)
                .Index(t => t.BoardSizeId)
                .Index(t => t.BlackPlayerId)
                .Index(t => t.WhitePlayerId)
                .Index(t => t.BlackPlayerRankId)
                .Index(t => t.WhitePlayerRankId)
                .Index(t => t.CreatorId)
                .Index(t => t.GameResultId);
            
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
            DropForeignKey("dbo.GameResult", "Winner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameResult", "GameId", "dbo.Game");
            DropForeignKey("dbo.Game", "WhitePlayerRankId", "dbo.Rank");
            DropForeignKey("dbo.Game", "WhitePlayerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game", "GameResultId", "dbo.GameResult");
            DropForeignKey("dbo.Game", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Game", "BoardSizeId", "dbo.BoardSize");
            DropForeignKey("dbo.Game", "BlackPlayerRankId", "dbo.Rank");
            DropForeignKey("dbo.Game", "BlackPlayerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Rank_Id", "dbo.Rank");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Rank_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Game", new[] { "GameResultId" });
            DropIndex("dbo.Game", new[] { "CreatorId" });
            DropIndex("dbo.Game", new[] { "WhitePlayerRankId" });
            DropIndex("dbo.Game", new[] { "BlackPlayerRankId" });
            DropIndex("dbo.Game", new[] { "WhitePlayerId" });
            DropIndex("dbo.Game", new[] { "BlackPlayerId" });
            DropIndex("dbo.Game", new[] { "BoardSizeId" });
            DropIndex("dbo.GameResult", new[] { "Winner_Id" });
            DropIndex("dbo.GameResult", new[] { "GameId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Rank");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Game");
            DropTable("dbo.GameResult");
            DropTable("dbo.BoardSize");
        }
    }
}
