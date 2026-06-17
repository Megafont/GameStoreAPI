namespace GameStore.API.DTOs;

// A DTO (Data Transfer Object) is a simple object that is used to transfer data between layers of an application.
// It is often used to transfer data from the database layer to the presentation layer, or from the presentation layer to the
// database layer. A DTO typically contains only the data that is needed for a specific operation, and does not contain any
// business logic or behavior. A DTO is a constract that defines how data is transferred between application layers, or between
// the client/server.


/// <summary>
/// A DTO that represents a game.
/// </summary>
/// <remarks>
/// This implementation of GameDTO is equivalent to the one below (GameDetailsDTO2), but it is more concise and easier to read.
/// It uses the new record type, which is a reference type that provides built-in functionality for value-based equality, immutability, and with-expressions.
/// The record type is ideal for DTOs because it allows us to easily create immutable objects that can be compared based on their values rather than their references.
/// </remarks>
public record GameDetailsDTO(
    int Id,
    string Title,
    int GenreId,
    decimal Price,
    DateOnly ReleaseData
);


/*
public record class GameDetailsDTO2
{
    public int Id { get; init; }
    public string Title { get; init; }
    public int GenreId { get; init; }
    public decimal Price { get; init; }
    public DateOnly ReleaseData { get; init; }
}
*/