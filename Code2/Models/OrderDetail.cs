using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Code2.Models
{
	public class OrderDetail
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("Book")]
		public int BookId { get; set; }
		public Book? Book { get; set; }
		[ForeignKey("Order")]
		public int OrderId { get; set; }
		public Order? Order { get; set; }
		[Column(TypeName = "decimal(8,2)")]
		public int Quantity { get; set; }
	}
}
