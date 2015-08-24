using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TournamentMaker.BO;
using TournamentMaker.BO.Tournaments;

namespace TournamentMaker.DAL
{
    public class MatchContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Pool> Pools { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Rank> Ranks { get; set; }

        public MatchContext()
            : base("MatchContext")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Team>().HasMany(t => t.Players).WithMany(m => m.Teams);
            modelBuilder.Entity<Tournament>().HasMany(t => t.Teams).WithMany(m => m.Tournaments);
            modelBuilder.Entity<Match>().HasMany(t => t.Teams).WithMany(m => m.Matches);
            modelBuilder.Entity<Rank>().HasRequired(r => r.Player).WithMany(p => p.Ranks);
            modelBuilder.Entity<Rank>().HasRequired(r => r.Sport).WithMany(p => p.Ranks);
            modelBuilder.Entity<Qualification>().HasOptional(q => q.NextQualification).WithMany(q => q.PreviousQualifications);
            modelBuilder.Entity<Qualification>().HasMany(q => q.Matchs).WithOptional(m => m.Qualification);

            modelBuilder.Entity<Tournament>().HasMany(t => t.Qualifications).WithRequired(q=>q.Tournament);
        }
    }
}