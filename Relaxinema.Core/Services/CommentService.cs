using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO.Comment;
using Relaxinema.Core.Helpers;
using Relaxinema.Core.Helpers.RepositoryParams;
using Relaxinema.Core.ServiceContracts;

namespace Relaxinema.Core.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IFilmRepository _filmRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CommentService(ICommentRepository commentRepository, IFilmRepository filmRepository, IUserRepository userRepository,  IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _filmRepository = filmRepository;
        _userRepository = userRepository;
    }

    public async Task<CommentResponse> GetByIdAsync(Guid id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment is null)
            throw new KeyNotFoundException("Cannot find comment with such id");

        return _mapper.Map<CommentResponse>(comment);
    }

    public async Task<PagedList<CommentResponse>> GetAllForFilmAsync(CommentParams commentParams)
    {
        return _mapper.Map<PagedList<CommentResponse>>(await _commentRepository.GetAllForFilmAsync(commentParams, new []{nameof(Comment.User)}));
    }

    public async Task<CommentResponse> CreateCommentAsync(CommentAddRequest commentAddRequest, Guid userId)
    {
        var user = await GetUser(userId);
        if (user is null)
            throw new KeyNotFoundException("Cannot find user with such id");

        var film = await GetFilm(commentAddRequest.FilmId.Value);

        var comment = _mapper.Map<Comment>(commentAddRequest);

        await _commentRepository.CreateAsync(comment);

        return _mapper.Map<CommentResponse>(comment);
    }

    public async Task<CommentResponse> UpdateCommentAsync(CommentUpdateRequest commentUpdateRequest)
    {
        var comment = _mapper.Map<Comment>(commentUpdateRequest);

        var response = await _commentRepository.UpdateAsync(comment);

        if (response is null)
            throw new KeyNotFoundException("Cannot find comment with such id");

        return _mapper.Map<CommentResponse>(response);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await _commentRepository.DeleteAsync(id))
            throw new KeyNotFoundException("Cannot find comment with such ");
    }
    
    private async Task<User> GetUser(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user is null)
            throw new KeyNotFoundException("Cannot find user with such id");
        
        return user;
    }

    private async Task<Film> GetFilm(Guid filmId)
    {
        var film = await _filmRepository.GetByIdAsync(filmId);
        
        if (film is null)
            throw new KeyNotFoundException("Cannot find film with such id");

        return film;
    }
}