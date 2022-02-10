namespace TinyBallot.Models;

public class CandidateDTO
{
    public int CandidateId { get; set; }
    public string Label { get; set; } = string.Empty;

    public CandidateDTO() { }
    public CandidateDTO(Candidate c) =>
        (CandidateId, Label) = (c.CandidateId, c.Label);
}
