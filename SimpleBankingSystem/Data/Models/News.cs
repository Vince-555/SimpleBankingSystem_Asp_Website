using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SimpleBankingSystem.Data.GlobalDataConstraints;

namespace SimpleBankingSystem.Data.Models
{
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(NewsTittleMaxLenght)]
        [MinLength(NewsTittleMinLength)]
        public string Title { get; set; } 

        [Required]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string PhotoUrl { get; set; }

    }
}
