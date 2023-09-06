using System.ComponentModel.DataAnnotations;

namespace Code2.Models
{
    public class GenreRequest
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Genre Name")]
        public string? GenreName { get; set; }

        [Display(Name = "Requested By User")]
        public string? RequestedByUserId { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }
    }
}
