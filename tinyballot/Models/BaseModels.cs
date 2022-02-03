using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TinyBallot.Validation;

namespace TinyBallot.Models;

public class Poll
{
    public int PollId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Description { get; set; } = string.Empty;

    [NotNullOrEmptyCollection]
    public ICollection<Candidate> Candidates { get; set; }
    public ICollection<Ballot> Ballots { get; set; }

    public Poll()
    {
	Candidates = new List<Candidate>();
	Ballots = new List<Ballot>();
    }
}

public class Ballot
{
    public int BallotId { get; set; }
    [Required]
    public int PollId { get; set; }
    public Poll? Poll { get; set; }

    [Required]
    public string Voter { get; set; } = string.Empty;
    public ICollection<BallotCandidate> BallotCandidates { get; set; }

    public Ballot()
    {
	BallotCandidates = new List<BallotCandidate>();
    }
}

public class Candidate
{
    public int CandidateId { get; set; }
    [Required]
    public int PollId { get; set; }
    public Poll? Poll { get; set; }
    
    public ICollection<BallotCandidate>? BallotCandidates { get; set; }
    
    [Required]
    public virtual string Label { get; set; } = string.Empty;
}

public class BallotCandidate
{
    public int BallotId { get; set; }
    public Ballot? Ballot { get; set; }
    
    public int CandidateId { get; set; }
    public Candidate? Candidate { get; set; }
}
