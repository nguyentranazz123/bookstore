using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Code2.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        [StringLength(300)]
        public string? Image { get; set; }
    }
}
