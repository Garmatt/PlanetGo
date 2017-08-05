using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CloudGoClub.Models
{
    public enum RankType
    {
        [Display(Name="Kyu")]
        KYU,
        [Display(Name="Dan")]
        DAN
    }

    public class Rank : IComparable<Rank>
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id { get; set; }

        [Required]
        public RankType Type { get; set; } 

        [Required]
        public short Level { get; set; }

        public int CompareTo(Rank other)
        {
            if (other == null)
                return 1;

            if (this.Type == RankType.DAN && other.Type == RankType.KYU)
                return 1;

            if (this.Type == RankType.KYU && other.Type == RankType.DAN)
                return -1;

            if (this.Type == RankType.KYU)
                return -(this.Level.CompareTo(other.Level));

            return this.Level.CompareTo(other.Level);
        }

        public static bool operator > (Rank a, Rank b)
        {
            return a != null && a.CompareTo(b) > 0;
        }

        public static bool operator >=(Rank a, Rank b)
        {
            return a != null && a.CompareTo(b) >= 0;
        }

        public static bool operator <(Rank a, Rank b)
        {
            return !(a >= b);
        }

        public static bool operator <=(Rank a, Rank b)
        {
            return !(a > b);
        }
    }

    public enum GameState
    {
        NEW,
        ONGOING,
        OVER,
        CANCELLED
    }

    public class BoardSize
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        public short X { get; set; }

        public short Y { get; set; }

        public string DisplayName { get { return X.ToString() + "x" + X.ToString(); } }
    }

    public enum GameResultType
    {
        NO_RESULT,
        BY_SCORE,
        BY_RESIGNATION
    }

    public class GameResult
    {
        [Required, Key]
        public long GameId { get; set; }

        [Required, ForeignKey("GameId")]
        public virtual Game Game { get; set; }

        [Required]
        public GameResultType Type { get; set; }

        public virtual ApplicationUser Winner { get; set; }

        public short? WinnerScore { get; set; }

        public short? LoserScore { get; set; }
    }

    public enum Color
    {
        BLACK,
        WHITE
    }

    public class Tournament
    {
        public int Id { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }

    public enum GameType
    {
        CORRESPONDENCE,
        REAL_TIME,
        HOT_SEAT
    }

    public enum HandicapStonesPlacement
    {
        FIXED,
        FREE
    }

    public class RuleSet
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public short Id { get; set; }
    }

    public class GameRequest
    {
        public GameRequest() { }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required, ForeignKey("AuthorId"), Display(Name="Player")]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        public bool IsOpen { get; set; }
        
        [Required, Display(Name="Created on")]
        public DateTime CreationTimestamp { get; set; }

        [Required]
        public GameType Type { get; set; }

        [Required]
        public short BoardSizeId { get; set; }

        [Required, ForeignKey("BoardSizeId"), Display(Name="Board size")]
        public virtual BoardSize BoardSize { get; set; }

        [Required]
        public short RuleSetId { get; set; }

        [Required, ForeignKey("RuleSetId"), Display(Name="Scoring rules")]
        public RuleSet RuleSet { get; set; }

        public Color? ColorOfAuthor { get; set; } //null means authomatic (based on rank difference)

        [Display(Name="Compensation points")]
        public double? CompensationPoints { get; set; } //null means authomatic (based on rank difference)

        [Required]
        public Color PlayerWithCompensationPoints { get; set; } //default is WHITE

        [Display(Name="Handicap stones")]
        public short? HandicapStones { get; set; } //null means authomatic (based on rank difference)

        [Required]
        public HandicapStonesPlacement HandicapStonesPlacement { get; set; } //default is FIXED

        public short? LowestOpponentRankId { get; set; } //null means authomatic (usually - 1)

        [ForeignKey("LowestOpponentRankId")]
        public virtual Rank LowestOpponentRank { get; set; }

        public short? HighestOpponentRankId { get; set; } //null means authomatic (usually + 1)

        [ForeignKey("HighestOpponentRankId")]
        public virtual Rank HighestOpponentRank { get; set; }

        public int? SpecificOpponentId { get; set; }

        [ForeignKey("SpecificOpponentId")]
        public ApplicationUser SpecificOpponent { get; set; }

        public bool NewOpponentsOnly { get; set; } //default is false

        public int? TournamentId { get; set; }

        [ForeignKey("TournamentId")]
        public virtual Tournament Tournament { get; set; }

        public long? GameId { get; set; }

        [ForeignKey("GameId")]
        public virtual Game Game { get; set; }
    }

    public class Game
    {
        public Game() { }

        [Required, Key]
        public long GameRequestId { get; set; }

        [Required, ForeignKey("GameRequestId")]
        public virtual GameRequest GameRequest { get; set; }

        public string SgfString { get; set; }

        [Required]
        public int BlackPlayerId { get; set; }

        [Required, ForeignKey("BlackPlayerId")]
        public virtual ApplicationUser BlackPlayer { get; set; }

        [Required]
        public int WhitePlayerId { get; set; }

        [Required, ForeignKey("WhitePlayerId")]
        public virtual ApplicationUser WhitePlayer { get; set; }

        [Required]
        public short BlackPlayerRankId { get; set; }

        [Required, ForeignKey("BlackPlayerRankId")]
        public virtual Rank BlackPlayerRank { get; set; }

        [Required]
        public short WhitePlayerRankId { get; set; }

        [Required, ForeignKey("WhitePlayerRankId")]
        public virtual Rank WhitePlayerRank { get; set; }

        [Required]
        public DateTime StartTimestamp { get; set; }

        public DateTime? LastPlayedTimestamp { get; set; }

        public DateTime? EndTimestamp { get; set; }

        [Required]
        public short Moves { get; set; }

        public Color? Turn { get; set; }

        public long? GameResultId { get; set; }

        [ForeignKey("GameResultId")]
        public virtual GameResult GameResult { get; set; }
    }
}