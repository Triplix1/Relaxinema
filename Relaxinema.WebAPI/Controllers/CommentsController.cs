using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO.Comment;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.WebAPI.Controllers.Base;

namespace Relaxinema.WebAPI.Controllers;

public class CommentsController : BaseController
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<CommentResponse>>> GetAllComments([FromQuery]CommentParams commentParams)
    {
        return Ok(await _commentService.GetAllForFilmAsync(commentParams));
    }

    [HttpPost("create")]
    public async Task<ActionResult<CommentResponse>> CreateComment([FromBody]CommentAddRequest commentAddRequest)
    {
        return Ok(await _commentService.CreateCommentAsync(commentAddRequest));
    }

    [HttpPut("edit")]
    public async Task<ActionResult<CommentResponse>> UpdateComment([FromBody]CommentUpdateRequest commentUpdateRequest)
    {
        return Ok(await _commentService.UpdateCommentAsync(commentUpdateRequest));
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete([FromRoute]Guid id)
    {
        await _commentService.DeleteAsync(id);
        return Ok();
    }
}