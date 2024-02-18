namespace TaskBoardApp.Models
{
    public class HomeViewModel
    {
        public int AllTasksCount { get; set; }

        public List<HomeBoardModel>
        BoardWithTasksCount { get; set; } = null!;

        public int UserTasksCount { get; set; }
    }
}
