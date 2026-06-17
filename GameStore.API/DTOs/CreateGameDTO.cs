using System.ComponentModel.DataAnnotations;

namespace GameStore.API.DTOs;

public record class CreateGameDTO
(
    [Required] [StringLength(50)] string Title,
    [Range(1, int.MaxValue)] int GenreId,
    [Range(0, 100)] decimal Price,
    DateOnly ReleaseDate
);
