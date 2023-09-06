﻿using System.ComponentModel.DataAnnotations;

namespace Code2.ViewModels
{
    public class EditGenreRequestViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }
    }
}
