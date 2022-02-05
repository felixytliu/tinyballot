using System.ComponentModel.DataAnnotations;

namespace TinyBallot.Models;

public class BallotDTO
{
    public int BallotId { get; set; }
    public int PollId { get; set; }
    public Poll? Poll { get; set; }

    [Required]
    public string? Voter { get; set; } = string.Empty;
    public ICollection<int> Candidates { get; set; }

    public BallotDTO() { Candidates = new List<int>(); }
    public BallotDTO(Poll p)
    {
	Poll = p;
        PollId = p.PollId;
	Candidates = new List<int>();
    }
}
