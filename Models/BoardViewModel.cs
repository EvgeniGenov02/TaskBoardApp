using System.ComponentModel.DataAnnotations;
using TaskBoardApp.Data;

namespace Task.BoardApp.Models
{
    public class BoardViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength
        (DataConstants.BoardConstants.NameMaxLength,
        MinimumLength = DataConstants.BoardConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<TaskViewModel> Tasks { get; set; }
        = new List<TaskViewModel>();

    }
}
