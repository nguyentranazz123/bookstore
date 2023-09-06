namespace Code2.Models
{
    public class CartLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Book? Book { get; set; }
    }

    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(Book book, int quantity)
        {
            CartLine? line = Lines
                .Where(b => b.Book?.Id == book.Id).FirstOrDefault();
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Book = book,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void DecreaseItem(Book book, int quantity)
        {
            CartLine? line = Lines.FirstOrDefault(b => b.Book?.Id == book.Id);
            if (line != null)
            {
                if (line.Quantity - quantity <= 0)
                {
                    RemoveLine(book);
                }
                else
                {
                    line.Quantity -= quantity;
                }
            }
        }


        public void RemoveLine(Book book)
        {
            Lines.RemoveAll(l => l.Book?.Id == book.Id);
        }

        public decimal ComputeTotalValue() =>
            Lines.Sum(e => e.Book.Price * e.Quantity);

        public void Clear() => Lines.Clear();
    }
}