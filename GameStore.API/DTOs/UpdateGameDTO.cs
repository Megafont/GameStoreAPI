namespace GameStore.API;

public record UpdateGameDTO
(
    string Title,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate    
);
