namespace UttuHub.API.DTOs.Project
{
    // UC 231 - Used when creating a new Project (POST /projects)
    // UserId links to the owner - will be taken from JWT claims once auth is added
    public class ProjectCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? TechStack { get; set; }
        public string? GithubUrl { get; set; }
        public string? LiveUrl { get; set; }


        public int UserId { get; set; } // TODO: Replace with JWT claim extraction once auth is added
    }
}

