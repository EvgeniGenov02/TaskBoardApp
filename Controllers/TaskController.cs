using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TaskBoardApp.Models;
using TaskBoardApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Task.BoardApp.Models;
using System.Security.Claims;
using TaskBoardApp.Data.Models;

namespace TaskBoardApp.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly TaskBoardAppDbContext data;

        public TaskController(TaskBoardAppDbContext context)
        {
            data = context;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        //Details
        public async Task<IActionResult> Details(int id)
        {
            var tasks = await data.Tasks
                .Where(t => t.Id == id)
                .Select(t => new TaskDetailsViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedOn = t.CreatedOn.ToString(),
                    Board = t.Board.Name,
                    Owner = t.Owner.UserName
                }).FirstOrDefaultAsync();

            if (tasks == null)
            {
                return BadRequest();
            }

            return View(tasks);
        }

        //Delete
        //Delete Task
        [HttpPost]
        public async Task<IActionResult> Delete(TaskViewModel taskModel)
        {
            var task = await data.Tasks.FindAsync(taskModel.Id);

            if (task == null)
            {
                return BadRequest();
            }
            string currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            data.Tasks.Remove(task);
            await data.SaveChangesAsync();

            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await data.Tasks.FindAsync(id);

            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            TaskViewModel taskModel = new()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
            };

            return View(taskModel);
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var tasks = await data.Tasks.FindAsync(id);

            if (tasks == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            if (currentUserId != tasks.OwnerId)
            {
                return Unauthorized();
            }

            TaskFormViewModel taskModel = new()
            {
                Title = tasks.Title,
                Description = tasks.Description,
                BoardId = (int)tasks.BoardId,
                Boards = await GetBoards(),
            };

            return View(taskModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskFormViewModel taskModel)
        {
            var task = await data.Tasks.FindAsync(id);

            if(task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();

            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();  
            }

            var board = await GetBoards();

            if (!board.Any(b => b.Id == taskModel.BoardId))
            {
                ModelState.AddModelError(nameof(taskModel.BoardId), "Board does not exist.");
            }

            if (ModelState.IsValid)
            {
                taskModel.Boards = await GetBoards();

                return View(taskModel);
            }

            task.Title = taskModel.Title;
            task.Description = taskModel.Description;
            task.BoardId = taskModel.BoardId;

            await data.SaveChangesAsync();

            return RedirectToAction("Index", "Board");
        }
        private async Task<IEnumerable<TaskBoardModel>> GetBoards()
        {
            return await data
             .Boards
             .AsNoTracking()
              .Select(b => new TaskBoardModel
              {
                  Id = b.Id,
                  Name = b.Name,
              }).ToListAsync();
        }

    }
}
