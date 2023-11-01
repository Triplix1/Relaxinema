using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Relaxinema.Core.Domain.Entities;

[PrimaryKey(nameof(FilmId), nameof(UserId))]
public class Rating
{
    public Guid FilmId { get; set; }
    public Guid UserId { get; set; }
    [Range(1,5)]
    public short Rate { get; set; }
    public User User { get; set; }
    public Film Film { get; set; }
}