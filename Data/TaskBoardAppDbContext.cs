using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data.Models;

namespace TaskBoardApp.Data
{
    public class TaskBoardAppDbContext : IdentityDbContext<IdentityUser>
    {
        public TaskBoardAppDbContext(DbContextOptions<TaskBoardAppDbContext> options)
         : base(options)
        {
            TestUser = new IdentityUser();
            OpenBoard = new Board();
            InProgressBoard = new Board();
            DoneBoard = new Board();
        }

        public DbSet<TaskBoardApp.Data.Models.Task> Tasks { get; set; }
        public DbSet<Board> Boards { get; set; }

        private IdentityUser TestUser { get; set; } = null!;
        private Board OpenBoard { get; set; } = null!;
        private Board InProgressBoard { get; set; } = null!;
        private Board DoneBoard { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
            .Entity<TaskBoardApp.Data.Models.Task>()
            .HasOne(t => t.Board)
            .WithMany(b => b.Tasks)
            .HasForeignKey(t => t.BoardId)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

            SeedUsers(builder);
            SeedBoards(builder);
            SeedInitialTasks(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            TestUser = new IdentityUser
            {
                UserName = "test@softuni.bg",
                NormalizedUserName = "TEST@SOFTUNI.BG",
                PasswordHash = hasher.HashPassword(TestUser, "softuni"),
            };

            var users = new List<IdentityUser> { TestUser };

            builder.Entity<IdentityUser>().HasData(users);
        }

        private void SeedBoards(ModelBuilder builder)
        {
            OpenBoard = new Board { Id = 1, Name = "Open" };
            InProgressBoard = new Board { Id = 2, Name = "In Progress" };
            DoneBoard = new Board { Id = 3, Name = "Done" };

            var boards = new List<Board> { OpenBoard, InProgressBoard, DoneBoard };

            builder.Entity<Board>().HasData(boards);
        }

        private void SeedInitialTasks(ModelBuilder builder)
        {
            var tasks = new List<TaskBoardApp.Data.Models.Task>
      {
        new TaskBoardApp.Data.Models.Task()
        {
          Id = 1,
          Title = "Improve CSS styles",
          Description = "Implement bettrr styling for all public pages",
          CreatedOn = DateTime.Now.AddDays(-200),
          OwnerId = TestUser.Id,
          BoardId = OpenBoard.Id,
        },
        new TaskBoardApp.Data.Models.Task()
        {
          Id = 2,
          Title = "Android Client App",
          Description = "Create Android client app for TaskBoard RESTful API",
          CreatedOn = DateTime.Now.AddDays(-5),
          OwnerId = TestUser.Id,
          BoardId = OpenBoard.Id,
        },
        new TaskBoardApp.Data.Models.Task()
        {
          Id = 3,
          Title = "Desktop Client App",
          Description = "Create Windows Forms desktop app client for the TaskBoard RESTful API",
          CreatedOn = DateTime.Now.AddDays(-1),
          OwnerId = TestUser.Id,
          BoardId = InProgressBoard.Id,
        },
         new TaskBoardApp.Data.Models.Task()
         {
             Id = 5,
             Title = "Implement User Authentication",
             Description = "Implement user authentication and authorization using ASP.NET Identity",
             CreatedOn = DateTime.Now,
             OwnerId = TestUser.Id,
             BoardId = DoneBoard.Id,
         },
         };
            builder.Entity<TaskBoardApp.Data.Models.Task>().HasData(tasks);
        }
   }
}

