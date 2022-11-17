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
    public class UserFavoriteController : ControllerBase
    {
        private readonly HelpDeskAppDbContext _context;

        public UserFavoriteController(HelpDeskAppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserFavorite
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFavorite>>> GetUserFavorites()
        {
          if (_context.UserFavorites == null)
          {
              return NotFound();
          }
            return await _context.UserFavorites.ToListAsync();
        }

        // GET: api/UserFavorite/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFavorite>> GetUserFavorite(int id)
        {
          if (_context.UserFavorites == null)
          {
              return NotFound();
          }
            //List<Ticket> bookmarkedTickets = _context.Tickets.Where(t => t.IsBookmarked == "true").ToList();
            var userFavorite = await _context.UserFavorites.FindAsync(id);

            if (userFavorite == null)
            {
                return NotFound();
            }

            return userFavorite;
        }

        // PUT: api/UserFavorite/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFavorite(int id,int userId, int ticketId)
        {
            UserFavorite userFavorite = _context.UserFavorites.FirstOrDefault(uf => uf.Id == id);

            if (userFavorite.Id != userFavorite.Id)
            {
                return BadRequest();
            }
            userFavorite.TicketId = ticketId;
            userFavorite.UserId = userId;
            _context.Entry(userFavorite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFavoriteExists(userFavorite.Id))
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

        // POST: api/UserFavorite
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserFavorite>> PostUserFavorite(int ticketId, int userId)
        {
            if (_context.UserFavorites == null)
            {
                return Problem("Entity set 'HelpDeskAppDbContext.UserFavorites'  is null.");
            }
            UserFavorite newUserFavorite = new UserFavorite()
            {
                UserId = userId,
                TicketId = ticketId,
                Ticket = _context.Tickets.FirstOrDefault(t => t.Id == ticketId),
                User = _context.Users.FirstOrDefault(u => u.Id == userId),
            };
            // change bookmarked status to true
            Ticket relevantTicket = _context.Tickets.FirstOrDefault(t => t.Id == ticketId);
            relevantTicket.IsBookmarked = "true";
            // add to user favorites
            _context.UserFavorites.Add(newUserFavorite);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserFavoriteExists(newUserFavorite.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserFavorite", new { id = newUserFavorite.Id }, newUserFavorite);
        }

        // DELETE: api/UserFavorite/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserFavorite(int id)
        {
            if (_context.UserFavorites == null)
            {
                return NotFound();
            }
            var userFavorite = await _context.UserFavorites.FindAsync(id);
            if (userFavorite == null)
            {
                return NotFound();
            }
            UserFavorite userFavorite1 = _context.UserFavorites.FirstOrDefault(u => u.Id == id);
            // change bookmarked status
            Ticket relevantTicket = _context.Tickets.FirstOrDefault(t => t.Id == userFavorite1.TicketId);
            relevantTicket.IsBookmarked = "false";
            _context.UserFavorites.Remove(userFavorite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserFavoriteExists(int id)
        {
            return (_context.UserFavorites?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
