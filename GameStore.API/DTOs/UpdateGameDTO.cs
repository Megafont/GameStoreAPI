using System.ComponentModel.DataAnnotations;

namespace GameStore.API.DTOs;

public record UpdateGameDTO
(
    [Required] [StringLength(50)] string Title,
    [Required] [StringLength(20)] string Genre,
    [Range(0, 100)] decimal Price,
    DateOnly ReleaseDate
);
