using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Relaxinema.Core.Domain.Entities
{
    public class Film
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public short? Year { get; set; }
        public short? Limitation { get; set; }
        public string? Description { get; set; }
        public bool Publish {  get; set; }
        public bool IsExpected {  get; set; }
        public string? PhotoUrl { get; set; }
        public string? PhotoPublicId { get; set; }
        public string? SourcesSerialized { get; set; }
        public string Trailer { get; set; } = null!;
        public DateTime Created { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<User> SubscribedUsers { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rating> Ratings { get; set; }

        [NotMapped]
        public string[] Sources
        {
            get { return JsonConvert.DeserializeObject<string[]>(SourcesSerialized); }
            set { SourcesSerialized = JsonConvert.SerializeObject(value); }
        }
    }
}
