using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskBoardApp.Data.Models
{
    public class Board
    {
        //· Id – a unique integer, Primary Key
        [Key]
        [Comment("Board Id")]
        public int Id { get; set; }

        //· Name – a string with min length 3 and max length 30 (required)
        [Required]
        [Comment("Board Name")]
        [MaxLength(DataConstants.BoardConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        //· Tasks – a collection of Task
        public IEnumerable<Task> Tasks { get; set; }
        = new List<Task>();
    }
}