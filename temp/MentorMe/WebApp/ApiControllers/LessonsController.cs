using App.DAL.Contracts;
using App.DAL.EF;
using Domain.Entities;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LessonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAppUOW _uow;

        public LessonsController(ApplicationDbContext context, IAppUOW uow)
        {
            _context = context;
            _uow = uow;
        }

        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetTrainingPlans()
        {
            var vm = await _uow.LessonsRepository.AllAsync(User.GetUserId());
            
            return Ok(vm);
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetLesson(Guid id)
        {
            var trainingPlan = await _uow.LessonsRepository.FindAsync(id, User.GetUserId());
            
            if (trainingPlan == null)
            {
                return NotFound();
            }

            return Ok(trainingPlan);
        }

        // PUT: api/Lessons/5
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutLesson(Guid id, Lesson lesson)
        // {
        //     if (id != lesson.Id)
        //     {
        //         return BadRequest();
        //     }
        //
        //     _context.Entry(lesson).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!TrainingPlanExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }
        //
        //     return NoContent();
        // }

        // POST: api/TrainingPlans
        // [HttpPost]
        // public async Task<ActionResult<Lesson>> PostLesson(Lesson lesson)
        // {
        //     lesson.AppUserId = User.GetUserId();
        //     
        //     _context.TrainingPlans.Add(trainingPlan);
        //     _uow.TrainingPlanRepository.Add(trainingPlan);
        //
        //     await _uow.SaveChangesAsync();
        //     
        //     return CreatedAtAction("GetTrainingPlan", new { id = trainingPlan.Id }, trainingPlan);
        // }

        // DELETE: api/TrainingPlans/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteLesson(Guid id)
        // {
        //     if (_context.TrainingPlans == null)
        //     {
        //         return NotFound();
        //     }
        //     var trainingPlan = await _context.TrainingPlans.FindAsync(id);
        //     if (trainingPlan == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.TrainingPlans.Remove(trainingPlan);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }

        // private bool TrainingPlanExists(Guid id)
        // {
        //     return (_context.TrainingPlans?.Any(e => e.Id == id)).GetValueOrDefault();
        // }
    }