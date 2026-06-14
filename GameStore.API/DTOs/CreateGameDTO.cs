namespace GameStore.API;

public record class CreateGameDTO
(
    string Title,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);