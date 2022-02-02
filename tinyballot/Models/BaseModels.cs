using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TinyBallot.Models;

public class Poll
{
    public int PollId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Required]
    public List<Candidate>? Candidates { get; set; }
    public List<Ballot>? Ballots { get; set; }
}

public class Ballot
{
    public int BallotId { get; set; }
    [Required]
    public Poll? Poll { get; set; }

    [Required]
    public string Voter { get; set; } = string.Empty;
    public IList<BallotCandidate>? BallotCandidates { get; set; }
}

public class Candidate
{
    public int CandidateId { get; set; }
    [Required]
    public Poll? Poll { get; set; }
    public IList<BallotCandidate>? BallotCandidates { get; set; }
    
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
