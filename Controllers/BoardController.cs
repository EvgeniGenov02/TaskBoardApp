using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task.BoardApp.Models;
using TaskBoardApp.Data;
using TaskBoardApp.Models;

namespace Task.BoardApp.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private readonly TaskBoardAppDbContext data;

        public BoardController(TaskBoardAppDbContext context)
        {
            data = context;
        }

        public async Task<IActionResult> Index()
        {
            var boards = await data.Boards
                .Select(b => new BoardViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Tasks = b.Tasks.Select(t => new TaskViewModel()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        CreatedOn = t.CreatedOn,
                        Owner = t.Owner.UserName,
                    })
                })
                .ToListAsync();

            return View(boards);
        }

        //Add new Task
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TaskFormViewModel taskModel = new TaskFormViewModel()
            {
                Boards = await GetBoards()
            };

            return View(taskModel);
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
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

        [HttpPost]
        public async Task<IActionResult> Create(TaskFormViewModel TaskFormModel)
        {
            var boards = await GetBoards();
            if (!boards.Any(b=> b.Id == TaskFormModel.BoardId))
            {
                ModelState.AddModelError(nameof(TaskFormModel.BoardId), "Board does not exist.");
            }

            string curntUserId = GetUserId();

            if(ModelState.IsValid)
            {
                TaskFormModel.Boards = await GetBoards();

                return View(boards);
            }

            TaskBoardApp.Data.Models.Task task = new()
            {
                Title = TaskFormModel.Title,
                Description = TaskFormModel.Description,
                CreatedOn = DateTime.Now,
                BoardId = TaskFormModel.BoardId,
                OwnerId = curntUserId,
            };

            await data.AddAsync(task);
            await data.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(TaskViewModel taskModel) 
        {
            var task = await data.Tasks.FindAsync(taskModel.Id);

            if (task != null)
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

            return RedirectToAction(nameof(Index));
        }


    }
}
