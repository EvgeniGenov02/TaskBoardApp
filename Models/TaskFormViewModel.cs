using System.ComponentModel.DataAnnotations;
using Task.BoardApp.Models;
using TaskBoardApp.Data;

namespace TaskBoardApp.Models
{
    public class TaskFormViewModel
    {
        [Required]
        [StringLength(DataConstants.TaskConstants.TitleMaxLength,
         MinimumLength = DataConstants.TaskConstants.TitleMinLength
            ,ErrorMessage = "Title shoud be lesst {2} characters lond.")]
        public string Title { get; set; } = null!;


        [Required]
        [StringLength(DataConstants.TaskConstants.DescriptionMaxLength,
        MinimumLength = DataConstants.TaskConstants.DescriptionMinLength
          , ErrorMessage = "Description shoud be lesst {2} characters lond.")]
        public string Description { get; set; } = null!;

        [Display(Name ="Board")]
        public int BoardId { get; set; }

        public IEnumerable<TaskBoardModel> Boards 
        { get; set; } = null!; 

    }
}
