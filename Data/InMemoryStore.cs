using Bookstore_.Models;

namespace Bookstore_.Data;

public static class InMemoryStore
{
    // In-memory users, books, and active JWT token queue.
    public static List<User> Users { get; } =
    [
        new User
        {
            Id = 1,
            Name = "Admin User",
            Email = "admin@bookstore.com",
            Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin"
        },
        new User
        {
            Id = 2,
            Name = "Regular User",
            Email = "user@bookstore.com",
            Password = BCrypt.Net.BCrypt.HashPassword("User@123"),
            Role = "User"
        }
    ];

    public static List<Book> Books { get; } =
    [
        new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Price = 29.99m },
        new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Price = 34.99m }
    ];

    public static List<string> ActiveTokens { get; } = [];
}
