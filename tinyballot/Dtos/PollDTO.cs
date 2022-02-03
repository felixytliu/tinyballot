namespace TinyBallot.Models;

public class PollViewDTO
{
    public int PollId { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public ICollection<Candidate>? Candidates { get; set; }

    public PollViewDTO() { }
    public PollViewDTO(Poll p) =>
	(PollId, Name, Description, Candidates) = (p.PollId, p.Name, p.Description, p.Candidates);
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
