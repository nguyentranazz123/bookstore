using System.ComponentModel.DataAnnotations;

namespace Code2.ViewModels
{
    public class CreateGenreRequestViewModel
    {
        [Required]
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }
    }
}
