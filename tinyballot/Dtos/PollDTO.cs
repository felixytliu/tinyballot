using System.ComponentModel.DataAnnotations;
using TinyBallot.Validation;

namespace TinyBallot.Models;

public class PollDTO
{
    public int PollId { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public ICollection<CandidateDTO>? Candidates { get; set; }
    public ICollection<BallotDTO>? Ballots { get; set; }

    public PollDTO(Poll p)
    {
        PollId = p.PollId;
        Name = p.Name;
        Description = p.Description;
        Candidates = (from c in p.Candidates
                      select new CandidateDTO(c)).ToList();
        Ballots = (from b in p.Ballots
                   select new BallotDTO(b)).ToList();
    }
}

public class PollHeaderDTO
{
    public int PollId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Description { get; set; } = string.Empty;

    [NotNullOrEmptyCollection]
    public ICollection<CandidateDTO> Candidates { get; set; } = new List<CandidateDTO>();

    public PollHeaderDTO() { }
    public PollHeaderDTO(Poll p) =>
	(PollId, Name, Description, Candidates) =
	(p.PollId, p.Name, p.Description, (from c in p.Candidates select new CandidateDTO(c)).ToList());
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
