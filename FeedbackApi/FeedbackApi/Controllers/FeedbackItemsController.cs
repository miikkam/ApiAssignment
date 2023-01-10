using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FeedbackApi.Models;

namespace FeedbackApi.Controllers
{
    [Route("/")]
    [ApiController]
    public class FeedbackItemsController : ControllerBase
    {
        private readonly FeedbackContext _context;

        public FeedbackItemsController(FeedbackContext context)
        {
            _context = context;
        }

        // GET: api/FeedbackItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackItemDTO>>> GetFeedbackItems()
        {
            return await _context.FeedbackItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/FeedbackItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackItemDTO>> GetFeedbackItem(long id)
        {
            var feedbackItem = await _context.FeedbackItems.FindAsync(id);

            if (feedbackItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(feedbackItem);
        }

        // PUT: api/FeedbackItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedbackItem(long id, FeedbackItemDTO feedbackDTO)
        {
            if (id != feedbackDTO.Id)
            {
                return BadRequest();
            }

            var feedbackItem = await _context.FeedbackItems.FindAsync(id);
            if (feedbackItem == null)
            {
                return NotFound();
            }

            feedbackItem.Name = feedbackDTO.Name;
            feedbackItem.Message = feedbackDTO.Message;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!FeedbackItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/FeedbackItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeedbackItemDTO>> PostFeedbackItem(FeedbackItemDTO feedbackDTO)
        {
            var feedbackItem = new FeedbackItem
            {
                Name = feedbackDTO.Name,
                Message = feedbackDTO.Message
            };

            _context.FeedbackItems.Add(feedbackItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetFeedbackItem),
                new { id = feedbackItem.Id },
                ItemToDTO(feedbackItem));
        }

        // DELETE: api/FeedbackItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedbackItem(long id)
        {
            var feedbackItem = await _context.FeedbackItems.FindAsync(id);
            if (feedbackItem == null)
            {
                return NotFound();
            }

            _context.FeedbackItems.Remove(feedbackItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedbackItemExists(long id)
        {
            return (_context.FeedbackItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static FeedbackItemDTO ItemToDTO(FeedbackItem feedbackItem) =>
            new FeedbackItemDTO
            {
                Id = feedbackItem.Id,
                Name = feedbackItem.Name,
                Message = feedbackItem.Message
            };
    }
}
