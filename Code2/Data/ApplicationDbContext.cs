using Code2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Code2.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Code2.Models.Author> Author { get; set; } = default!;

		public DbSet<Code2.Models.Book>? Book { get; set; }

		public DbSet<Code2.Models.Genre>? Genre { get; set; }

		public DbSet<Code2.Models.Order>? Order { get; set; }

		public DbSet<Code2.Models.OrderDetail>? OrderDetail { get; set; }
		public DbSet<Code2.Models.GenreRequest>? GenreRequest { get; set; }


		public DbSet<Code2.ViewModel.UserRoleViewModel>? UserRoleViewModel { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Author>().HasData(
				new Author { Id = 1, Name = "Aoyama Gosho" },
				new Author { Id = 2, Name = "William Shakespeare" },
				new Author { Id = 3, Name = "Bridgett Devoue" },
				new Author { Id = 4, Name = "D. Barkley Briggs" },
				new Author { Id = 5, Name = "Deborah Lerme Goodman" }
			);

			modelBuilder.Entity<Genre>().HasData(
				new Genre { Id = 1, Name = "Comic" },
				new Genre { Id = 2, Name = "Novel" },
				new Genre { Id = 3, Name = "Drama" },
				new Genre { Id = 4, Name = "Poems" },
				new Genre { Id = 5, Name = "Math" },
				new Genre { Id = 6, Name = "Fiction" }
			);

			modelBuilder.Entity<Book>().HasData(
				new Book
				{
					Id = 1,
					Name = "Conan",
					Price = 100,
					Image = "Conan.jpg",
					GenreId = 1,
					AuthorId = 1,
					Description =
						"The manga series has sold over 140 million copies in Japan and adapted into various platforms like anime, trading card games etc. The manga is published by Shogakukan."
				},
				new Book
				{
					Id = 2,
					Name = "Romeo And Juliet",
					Price = 120,
					Image = "RomeoAndJuliet.jpg",
					GenreId = 2,
					AuthorId = 2,
					Description =
						"It was among Shakespeare's most popular plays during his lifetime and, along with Hamlet"
				},
				new Book
				{
					Id = 3,
					Name = "SoftThorns",
					Price = 110,
					Image = "SoftThorns.jpg",
					GenreId = 4,
					AuthorId = 3,
					Description =
						@"The poetry living within these pages tells stories of love, heartbreak, freedom, oppression, sexual assault, sexism, hope and humanity. our darkest times are where we grow the most, so in this book, i share mine, and together, we learn how to heal."
				},
				new Book
				{
					Id = 4,
					Name = "The War of Swords",
					Price = 100,
					Image = "TheWarofSwords.jpg",
					GenreId = 6,
					AuthorId = 4,
					Description =
						@"Having laid his noose around the neck of Karac Tor, Kr’Nunos now begins to tighten the rope. A vast armada sails south, ravers press toward Midland."
				},
				new Book
				{
					Id = 5,
					Name = "The Magic of Unicorns",
					Price = 120,
					Image = "TheMagicOfUnicorn.jpg",
					GenreId = 4,
					AuthorId = 5,
					Description =
						@"This 4-book boxed set of interactive, children''s classics includes 4 books from the Choose Your Own Adventure * The Magic of the Unicorn * The Throne of Zeus * The Trumpet of Terror * Forecast From Stonehenge."
				}
			);
		}
	}
}