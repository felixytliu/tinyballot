#nullable disable

using Microsoft.EntityFrameworkCore;
using TinyBallot.Models;

namespace TinyBallot.Data;

public class SimplePollContext : DbContext
{
    public SimplePollContext(DbContextOptions<SimplePollContext> options)
	: base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
	modelBuilder.Entity<BallotCandidate>()
	    .HasKey(bc => new { bc.BallotId, bc.CandidateId });

	modelBuilder.Entity<BallotCandidate>()
	    .HasOne<Ballot>(bc => bc.Ballot)
	    .WithMany(b => b.BallotCandidates)
	    .HasForeignKey(bc => bc.BallotId)
	    .OnDelete(DeleteBehavior.ClientCascade);

	modelBuilder.Entity<BallotCandidate>()
	    .HasOne<Candidate>(bc => bc.Candidate)
	    .WithMany(c => c.BallotCandidates)
	    .HasForeignKey(bc => bc.CandidateId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }

    public DbSet<Poll> Polls { get; set; }
    public DbSet<Ballot> Ballots { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<BallotCandidate> BallotCandidates { get; set; }
}
