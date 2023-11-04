﻿namespace Relaxinema.Core.DTO.Comment;

public class CommentResponse
{
    public Guid Id { get; set; }
    public Guid FilmId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public string Text { get; set; }
}