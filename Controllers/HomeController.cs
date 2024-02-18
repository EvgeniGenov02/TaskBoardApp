using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using TaskBoardApp.Data;
using TaskBoardApp.Models;

namespace TaskBoardApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly TaskBoardAppDbContext data;

        public HomeController(TaskBoardAppDbContext context)
        {
            data = context;
        }

        public async Task<IActionResult> Index()
        {
            var taskBoards = await data.Boards
                .Include(b => b.Tasks)
                .Select(b => b.Name)
                .Distinct()
                .ToListAsync();

            var tasksCount = new List<HomeBoardModel>();

            foreach (var boardName in taskBoards)
            {
                var tasksInBoard = 0;

                foreach (var task in data.Tasks.Where(t => t.Board.Name == boardName))
                {
                    tasksInBoard++;
                }

                tasksCount.Add(new HomeBoardModel()
                {
                    BoardName = boardName,
                    TasksCount = tasksInBoard,
                });
            }

            var userTasksCount = -1;

            if (User.Identity.IsAuthenticated)
            {
                var curntUserId = User
                    .FindFirst(ClaimTypes.NameIdentifier)
                    .Value;

                userTasksCount = await data.Tasks
                    .Where(t => t.OwnerId == curntUserId)
                    .CountAsync();
            }

            var homeModel = new HomeViewModel()
            {
                AllTasksCount = data.Tasks.Count(),
                BoardWithTasksCount = tasksCount,
                UserTasksCount = userTasksCount
            };

            return View(homeModel);
        }

    }
}
