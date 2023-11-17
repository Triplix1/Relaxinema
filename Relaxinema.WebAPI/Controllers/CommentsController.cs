using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO.Comment;
using Relaxinema.Core.Extentions;
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
    public async Task<ActionResult<PagedList<CommentResponse>>> GetAllComments([FromHeader]CommentParams commentParams)
    {
        var result = await _commentService.GetAllForFilmAsync(commentParams);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<CommentResponse>> CreateComment([FromBody]CommentAddRequest commentAddRequest)
    {
        var userId = User.GetUserId();
        return Ok(await _commentService.CreateCommentAsync(commentAddRequest, userId));
    }

    [Authorize]
    [HttpPut("edit")]
    public async Task<ActionResult<CommentResponse>> UpdateComment([FromBody]CommentUpdateRequest commentUpdateRequest)
    {
        var userId = User.GetUserId();
        return Ok(await _commentService.UpdateCommentAsync(commentUpdateRequest, userId));
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete([FromRoute]Guid id)
    {
        await _commentService.DeleteAsync(id);
        return Ok();
    }
}