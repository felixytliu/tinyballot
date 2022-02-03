using System.ComponentModel.DataAnnotations;
using TinyBallot.Validation;

namespace TinyBallot.Models;

public class PollDTO
{
    public int PollId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Description { get; set; } = string.Empty;

    [NotNullOrEmptyCollection]
    public ICollection<Candidate>? Candidates { get; set; }

    public PollDTO() { Candidates = new List<Candidate>(); }
    public PollDTO(Poll p) =>
	(PollId, Name, Description, Candidates) =
	(p.PollId, p.Name, p.Description, p.Candidates);
}

public class PollBriefDTO
{
    public int PollId { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public int Candidates { get; set; }
    public int Ballots { get; set; }

    public PollBriefDTO() { }
    public PollBriefDTO(Poll p) =>
	(PollId, Name, Description, Candidates, Ballots) =
	(p.PollId, p.Name, p.Description, p.Candidates.Count, p.Ballots.Count);
}
