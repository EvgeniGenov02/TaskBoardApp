using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBoardApp.Data.Models
{
    public class Task
    {
        //Id – a unique integer, Primary Key

        [Key]
        [Comment("Task id")]
        public int Id { get; set; }

        //Title – a string with min length 5 and max length 70 (required)
        [Required]
        [MaxLength(DataConstants.TaskConstants.TitleMaxLength)]
        [Comment("Task Title")]
        public string Title { get; set; } = null!;

        //Description – a string with min length 10 and max length 1000 (required)
        [Required]
        [MaxLength(DataConstants.TaskConstants.DescriptionMaxLength)]
        [Comment("Task Description")]
        public string Description { get; set; } = null!;

        //CreatedOn – date and time
        [Comment("Task created on date")]
        public DateTime? CreatedOn { get; set; }

        //BoardId – an integer
        [Comment("Board Id")]
        public int? BoardId { get; set; }

        //OwnerId – an string(required)
        [Required]
        [Comment("Owner Id")]
        public string OwnerId { get; set; } = null!;

        //Board – a Board object
        [ForeignKey(nameof(BoardId))]
        public Board? Board { get; set; }


        //Owner – an IdentityUser object
        [ForeignKey(nameof(OwnerId))]
        public IdentityUser Owner { get; set; } = null!;
    }
}
