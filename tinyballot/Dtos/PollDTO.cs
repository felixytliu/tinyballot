using System.ComponentModel.DataAnnotations;
using TinyBallot.Validation;

namespace TinyBallot.Models;

public class PollEditDTO
{
    public int PollId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Description { get; set; } = string.Empty;

    [NotNullOrEmptyCollection]
    public ICollection<Candidate>? Candidates { get; set; }

    public PollEditDTO() { Candidates = new List<Candidate>(); }
}

public class PollOverviewDTO
{
    public int PollId { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public int Candidates { get; set; }
    public int Ballots { get; set; }

    public PollOverviewDTO() { }
    public PollOverviewDTO(Poll p) =>
	(PollId, Name, Description, Candidates, Ballots) =
	(p.PollId, p.Name, p.Description, p.Candidates.Count, p.Ballots.Count);
}
