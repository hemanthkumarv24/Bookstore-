using Bookstore_.Data;
using Bookstore_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore_.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(InMemoryStore.Books);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        return book is null ? NotFound() : Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Create(Book request)
    {
        var book = new Book
        {
            Id = InMemoryStore.Books.Count == 0 ? 1 : InMemoryStore.Books.Max(b => b.Id) + 1,
            Title = request.Title,
            Author = request.Author,
            Price = request.Price
        };

        InMemoryStore.Books.Add(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Update(int id, Book request)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
        {
            return NotFound();
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.Price = request.Price;

        return Ok(book);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var book = InMemoryStore.Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
        {
            return NotFound();
        }

        InMemoryStore.Books.Remove(book);
        return NoContent();
    }
}
