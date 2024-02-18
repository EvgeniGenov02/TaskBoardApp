using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskBoardApp.Data;

namespace Task.BoardApp.Models
{
    public class TaskViewModel
    {

        public int Id { get; set; }


        [Required]
        [StringLength(DataConstants.TaskConstants.TitleMaxLength,
         MinimumLength = DataConstants.TaskConstants.TitleMinLength)]
        public string Title { get; set; } = null!;


        [Required]
        [StringLength(DataConstants.TaskConstants.DescriptionMaxLength,
        MinimumLength = DataConstants.TaskConstants.DescriptionMinLength)]
        public string Description { get; set; } = null!;


        public DateTime? CreatedOn { get; set; }

        public int? BoardId { get; set; }


        [Required]
        public string Owner { get; set; } = null!;
    }
}
