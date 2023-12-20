namespace Relaxinema.Core.Helpers.RepositoryParams
{
    public class FilmParams : PaginatedParams
    {
        public FilterParams? FilterParams { get; set; }
        public OrderByParams? OrderByParams { get; set; }
        public string? Search { get; set; }
        public bool ShowHiddens { get; set; } = false;
    }
}
