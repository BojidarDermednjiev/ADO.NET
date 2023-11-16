namespace P02_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    using Common;

    public class Team
    {
        public Team()
        {
            this.HomeGames = new HashSet<Game>();
            this.AwaGames = new HashSet<Game>();
            this.Players = new HashSet<Player>();
        }

        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TeamNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.TeamLogoUrlMaxLength)]
        public string LogoUrl { get; set; } = null!;

        [MaxLength(ValidationConstants.TeamInitialMaxLength)]
        public string Initials { get; set; } = null!;
        public decimal Budget { get; set; }

        [ForeignKey(nameof(PrimaryKitColor))]
        public int PrimaryKitColorId { get; set; }
        public virtual Color PrimaryKitColor { get; set; }

        [ForeignKey(nameof(SecondaryKitColor))]
        public int SecondaryKitColorId { get; set; }
        public virtual Color SecondaryKitColor { get; set; }

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public virtual Town Town { get; set; }

        [InverseProperty(nameof(Game.HomeTeam))]
        public virtual ICollection<Game> HomeGames{ get; set; }

        [InverseProperty(nameof(Game.AwayTeam))]
        public virtual ICollection<Game> AwaGames { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}