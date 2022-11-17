using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDeskApi;

namespace HelpDeskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly HelpDeskAppDbContext _context;

        public TicketController(HelpDeskAppDbContext context)
        {
            _context = context;
        }

        // GET: api/Ticket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            return await _context.Tickets.ToListAsync();
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Ticket/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, string title, string dateSubmitted, string priority, string details, string resolvedBy, string resolutionNote, string active, string isBookmarked)
        {
            Ticket ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
            if (id != ticket.Id)
            {
                return BadRequest();
            }
            ticket.Title = title;
            ticket.DateSubmitted = dateSubmitted;
            ticket.Priority = priority;
            ticket.Details = details;
            ticket.ResolvedBy = resolvedBy;
            ticket.ResolutionNote = resolutionNote;
            ticket.Active = active;
            ticket.IsBookmarked = isBookmarked;

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ticket
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(string _title, string _dateSubmitted, string _priority, string _details, int _userId)
        {
            Ticket ticket = new Ticket
            {
                Title = _title,
                DateSubmitted = _dateSubmitted,
                Priority = _priority,
                Details = _details,
                SubmittedBy = _context.Users.FirstOrDefault(u => u.Id == _userId).Name,
                UserId = _userId,
                ResolvedBy = "",
                ResolutionNote = "",
                Active = "true",
                IsBookmarked = "false",
                User = _context.Users.FirstOrDefault(u => u.Id == _userId),

            };
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'HelpDeskAppDbContext.Tickets'  is null.");
            }
            _context.Tickets.Add(ticket);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TicketExists(ticket.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            if (_context.Tickets == null)
            {
                return NotFound();
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
