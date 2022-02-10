#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinyBallot.Data;
using TinyBallot.Models;

namespace tinyballot.Controllers
{
    [ApiController]
    [Route("api")]
    public class SimplePollAPIController : ControllerBase
    {
        private readonly SimplePollContext _context;

        public SimplePollAPIController(SimplePollContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<ICollection<PollBriefDTO>>> Index()
        {
            var polls = await _context.Polls
                .Include(p => p.Candidates)
                .Include(p => p.Ballots)
                .Select(p => new PollBriefDTO(p))
                .ToListAsync();
            
            return polls;
        }

        // Get: 5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollHeaderDTO>> HeaderGet(int id)
        {
            var poll = await _context.Polls
                .Include(p => p.Candidates)
                .AsSingleQuery()
                .FirstOrDefaultAsync(p => p.PollId == id);

            if (poll == null)
                return NotFound();
            
            return new PollHeaderDTO(poll);
        }

        // GET: poll/5
        [HttpGet("poll/{id}")]
        public async Task<ActionResult<PollDTO>> PollGet(int id)
        {
            var poll = await _context.Polls
                .Include(p => p.Candidates)
                .Include(p => p.Ballots)
                .ThenInclude(b => b.BallotCandidates)
                .AsSingleQuery()
                .FirstOrDefaultAsync(p => p.PollId == id);
            
            if (poll == null)
                return NotFound();

            return new PollDTO(poll);
        }

        // POST: SimplePoll/Create
        [HttpPost("poll")]
        public async Task<ActionResult<PollHeaderDTO>> PollPost(PollHeaderDTO pollDTO)
        {
            if (ModelState.IsValid)
            {
                Poll poll = new Poll
                {
                    Name = pollDTO.Name,
                    Description = pollDTO.Description,
                    Candidates = (from c in pollDTO.Candidates
                                  select new Candidate()
                                  {
                                      Label = c.Label
                                  }).ToList()
                };
                _context.Add(poll);
                await _context.SaveChangesAsync();

                return CreatedAtAction("poll",
                                       new { id = poll.PollId },
                                       new PollHeaderDTO(poll));
            }

            return BadRequest();
        }

        // POST: poll/5
        [HttpPost("poll/{id}")]
        public async Task<ActionResult<PollHeaderDTO>> PollPost(int id, PollHeaderDTO pollDTO)
        {
            var poll = await _context.Polls
                .Include(p => p.Candidates)
                .ThenInclude(c => c.BallotCandidates)
                .Include(p => p.Ballots)
                .ThenInclude(b => b.BallotCandidates)
                .FirstOrDefaultAsync(p => p.PollId == id);

            if (poll == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    poll.Name = pollDTO.Name;
                    poll.Description = pollDTO.Description;

                    // keep given candidates that were there already
                    var newCandidates = (from c in poll.Candidates
                                         where pollDTO.Candidates.Any(cDTO => cDTO.CandidateId == c.CandidateId)
                                         select c
                    ).ToList();
                    foreach (var cDTO in pollDTO.Candidates)
                    {
                        var tmpC = newCandidates.FirstOrDefault(c => c.CandidateId == cDTO.CandidateId);
                        if (tmpC == null)
                        {
                            // If new candidate, just add them to list
                            newCandidates.Add(new Candidate(){ Label = cDTO.Label });
                        }
                        else
                        {
                            // If candidate was already there, update it
                            tmpC.Label = cDTO.Label;
                        }
                    }
                    // update poll with new candidate list
                    poll.Candidates = newCandidates;

                    _context.Update(poll);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("poll",
                                           new { id = poll.PollId },
                                           new PollDTO(poll));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(pollDTO.PollId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return BadRequest();
        }

        // DELETE: poll/5
        [HttpDelete("poll/{id}")]
        public async Task<IActionResult> PollDelete(int id)
        {
            var poll = await _context.Polls
                .Include(p => p.Candidates)
                .Include(p => p.Ballots)
                .ThenInclude(b => b.BallotCandidates)
                .AsSingleQuery()
                .FirstOrDefaultAsync(p => p.PollId == id);
            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.PollId == id);
        }

        // GET: ballot/5
        [HttpGet("ballot/{id}")]
        public async Task<ActionResult<BallotDTO>> BallotGet(int id)
        {
            var ballot = await _context.Ballots
                .Include(b => b.BallotCandidates)
                .AsSingleQuery()
                .FirstOrDefaultAsync(b => b.BallotId == id);

            if (ballot == null)
                return NotFound();

            return new BallotDTO(ballot);
        }
        
        // POST: ballot/5
        [HttpPost("ballot/{id}")]
        public async Task<ActionResult<BallotDTO>> BallotPost(int id, BallotDTO ballotDTO)
        {
            var poll = await _context.Polls
                .Include(p => p.Ballots)
                .AsSingleQuery()
                .FirstOrDefaultAsync(p => p.PollId == id);

            if (poll == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var ballot = new Ballot()
                    {
                        PollId = id,
                        Voter = ballotDTO.Voter,
                        BallotCandidates = (from c in ballotDTO.Candidates
                            select new BallotCandidate()
                                {
                                    BallotId = ballotDTO.BallotId,
                                    CandidateId = c
                                }
                        ).ToList()
                    };

                    poll.Ballots.Add(ballot);
                    _context.Update(poll);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("ballot",
                                           new { id = ballot.BallotId },
                                           new BallotDTO(ballot));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(poll.PollId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            return BadRequest();
        }
    }
}
