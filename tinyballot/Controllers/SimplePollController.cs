#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TinyBallot.Data;
using TinyBallot.Models;

namespace tinyballot.Controllers
{
    public class SimplePollController : Controller
    {
        private readonly SimplePollContext _context;

        public SimplePollController(SimplePollContext context)
        {
            _context = context;
        }

        // GET: SimplePoll
        public async Task<IActionResult> Index()
        {
	    var polls = await _context.Polls
		.Include(p => p.Candidates)
		.Include(p => p.Ballots)
		.Select(p => new PollOverviewDTO(p))
		.ToListAsync();
	    
            return View(polls);
        }

        // GET: SimplePoll/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .FirstOrDefaultAsync(m => m.PollId == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        // GET: SimplePoll/Create
        public IActionResult Create()
        {
	    var pollDTO = new PollEditDTO();
	    
            return View(pollDTO);
        }

        // POST: SimplePoll/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PollId,Name,Description,Candidates")] PollEditDTO pollDTO)
        {
            if (ModelState.IsValid)
            {
		Poll poll = new Poll
		{
		    PollId = pollDTO.PollId,
		    Name = pollDTO.Name,
		    Description = pollDTO.Description,
		    Candidates = pollDTO.Candidates
		};
                _context.Add(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pollDTO);
        }

	public IActionResult AddCandidate()
	{
	    return PartialView("CandidateRow", new Candidate());
	}
	
        // GET: SimplePoll/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }

        // POST: SimplePoll/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PollId,Name,Description")] Poll poll)
        {
            if (id != poll.PollId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poll);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }

        // GET: SimplePoll/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Polls
                .FirstOrDefaultAsync(m => m.PollId == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        // POST: SimplePoll/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.PollId == id);
        }
    }
}
