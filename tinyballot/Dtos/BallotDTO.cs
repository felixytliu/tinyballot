using System.ComponentModel.DataAnnotations;

namespace TinyBallot.Models;

public class BallotDTO
{
    public int BallotId { get; set; }
    public int PollId { get; set; }

    [Required]
    public string? Voter { get; set; } = string.Empty;
    public ICollection<int> Candidates { get; set; }

    public BallotDTO() { Candidates = new List<int>(); }
    public BallotDTO(Ballot b) =>
        (BallotId, PollId, Voter, Candidates) = (b.BallotId, b.PollId, b.Voter, (from c in b.BallotCandidates select c.CandidateId).ToList());
}
